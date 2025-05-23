using AprobacionProyectos.Application.DTOs.EntitiesDTOs;
using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Application.DTOs.Response;
using AprobacionProyectos.Application.Helpers;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Application.Mappers;
using AprobacionProyectos.Application.Validators;
using AprobacionProyectos.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectProposalCreatorService _creatorService;
        private readonly IProjectProposalQueryService _queryService;
        private readonly IUserService _userService;
        private readonly IApprovalWorkflowService _approveWorkflowService;
        private readonly IProjectProposalUpdateService _updaterService;
        private readonly IValidator<ProjectQueryRequestDto> _queryValidator;


        public ProjectController(
            IProjectProposalCreatorService creatorService,
            IProjectProposalQueryService queryService,
            IUserService userService,
            IApprovalWorkflowService approveWorkflowService,
            IProjectProposalUpdateService updaterService,
            IValidator<ProjectQueryRequestDto> queryValidator
            )
        {
            _creatorService = creatorService;
            _queryService = queryService;
            _userService = userService;
            _approveWorkflowService = approveWorkflowService;
            _updaterService = updaterService;
            _queryValidator = queryValidator;
        }

        [HttpPost] //criterio 1 : crear propuesta de proyecto
        public async Task<IActionResult> Create([FromBody] CreateProjectProposalRequestDto dto) 
        {
            
            var existing = await _queryService.GetProjectProposalByTitle(dto.Title); // no hay repeticion de títulos 
            if (existing != null)
            {
                return Conflict(new { message = "Ya existe un proyecto con ese título." });
            }

            var proposal = await _creatorService.BuildAsync(dto);  //creo la propuesta base con el dto

            var id = await _creatorService.CreateProjectProposalAsync(proposal); //creo el proyecto completo 

            var proposalCreated = await _queryService.GetProjectProposalFullWithId(id); //obtengo el proyecto creado

            if (proposalCreated != null)
            {
                return CreatedAtAction(nameof(GetById), new { id }, ProjectProposalMapper.ToDto(proposalCreated)); //devuelvo el proyecto creado mapeado
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet] // criterio 2
        public async Task<IActionResult> GetProjects([FromQuery] ProjectQueryRequestDto filter)  
        {
            var validationResult = await _queryValidator.ValidateAsync(filter); //valido los filtros

            if (!validationResult.IsValid)
            {
                return BadRequest(new{ 
                    errors = validationResult.Errors
                        .GroupBy(e => e.PropertyName) //para agrupar los errores de forma más legible
                        .ToDictionary(
                            g => g.Key, 
                            g => g.Select(e => e.ErrorMessage).ToArray())
                });
            }

            var query = _queryService.GetProjectProposalQueryable(); //obtengo la query de proyectos

            var filteredQuery = await ProjectProposalAsyncFilter.ApplyFiltersAsync(query, filter, _userService);  //aplico los filtros a la query de proyectos

            var result = await filteredQuery.Select(p => ProjectQueryMapper.ToDto(p)).ToListAsync(); //aplico el mapeo 

            return Ok(result); //devuelvo el resultado

        }

        [HttpPost("{id}/decision")] //criterio 3
        public async Task<IActionResult> MakeDecision(Guid id, [FromBody] DecisionStepRequestDto decision)
        {
            // Validación opcional para verificar si el proyecto existe
            var projectProposal =  await _queryService.GetProjectProposalByIdAsync(id);
            if (projectProposal == null)
            {
                return NotFound(new { message = "Proyecto no encontrado" });
            }

            var success = await _approveWorkflowService.ApproveStepAsync(decision.Id, decision.User, decision.Status , decision.Observation);

            if (!success)
            {
                return Conflict(new { message = "El proyecto ya no se encuentra en un estado que permite modificaciones" });
            }

            var proposal = await _queryService.GetProjectProposalFullWithId(id);

            return Ok(ProjectProposalMapper.ToDto(proposal));
        }


        [HttpGet("{id}")] //criterio 4 
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!Guid.TryParse(id, out var guid) || guid == Guid.Empty) //valido el guid 
            {
                return BadRequest(new { message = "Id inválido" });
            }

            var proposal = await _queryService.GetProjectProposalFullWithId(guid);

            if (proposal == null)
                return NotFound(new { message = "Proyecto no encontrado" });

            return Ok(ProjectProposalMapper.ToDto(proposal)); //devuelvo el proyecto mapeado

        }


        [HttpPatch("{id}")] //criterio 5 → resta hacer fluentValidation y solid
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateProjectProposalRequestDto dto)
        {
            if (!Guid.TryParse(id, out var guid) || guid == Guid.Empty) //valido el guid 
            {
                return BadRequest(new { message = "El ID del proyecto es inválido." });
            }

            var projectProposal = await _queryService.GetProjectProposalFullWithId(guid);

            if (projectProposal == null)
            {
                return NotFound(new { message = "No se encontró un proyecto con ese ID." });
            }

            if (projectProposal.Status.Id != 4 )  //solo se puede editar un proyecto en estado de observacion(parte de la consigna)
            {
                return Conflict(new { message = "El proyecto ya no se encuentra en un estado que permite modificaciones" });
            }

            //aplico cambios (creé un updater solo para este caso)
            var updatedProposal = await _updaterService.UpdateProjectProposalAsync(guid, dto.title, dto.description, dto.duration);

            return Ok(ProjectProposalMapper.ToDto(updatedProposal)); //mapeo como resultado

        }
    }
}

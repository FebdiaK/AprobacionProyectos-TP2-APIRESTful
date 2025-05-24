using AprobacionProyectos.Application.DTOs.EntitiesDTOs;
using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Application.DTOs.Response;
using AprobacionProyectos.Application.Helpers;
using AprobacionProyectos.Application.Interfaces.ServicesInterfaces;
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
        private readonly IValidator<DecisionStepRequestDto> _decisionValidator;
        private readonly IValidator<CreateProjectProposalRequestDto> _createProjectProposalValidator;
        private readonly IValidator<UpdateProjectProposalRequestDto> _updateProjectProposalValidator;

        public ProjectController(
            IProjectProposalCreatorService creatorService,
            IProjectProposalQueryService queryService,
            IUserService userService,
            IApprovalWorkflowService approveWorkflowService,
            IProjectProposalUpdateService updaterService,
            IValidator<ProjectQueryRequestDto> queryValidator,
            IValidator<DecisionStepRequestDto> decisionValidator,
            IValidator<CreateProjectProposalRequestDto> createProjectProposalValidator,
            IValidator<UpdateProjectProposalRequestDto> updateProjectProposalValidator)
        {
            _creatorService = creatorService;
            _queryService = queryService;
            _userService = userService;
            _approveWorkflowService = approveWorkflowService;
            _updaterService = updaterService;
            _queryValidator = queryValidator;
            _decisionValidator = decisionValidator;
            _createProjectProposalValidator = createProjectProposalValidator;
            _updateProjectProposalValidator = updateProjectProposalValidator;
        }




        [HttpPost] //criterio 1 : crear propuesta de proyecto 
        public async Task<IActionResult> Create([FromBody] CreateProjectProposalRequestDto dto)
        {
            var validationResult = await _createProjectProposalValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return ValidationHelper.FromFluentValidationResult(validationResult);

            try
            {
                var proposal = await _creatorService.BuildAsync(dto);  //creo el proyecto con el dto y valido si no hay duplicacion de titulos

                var id = await _creatorService.CreateProjectProposalAsync(proposal); //creo el proyecto completo

                var proposalCreated = await _queryService.GetProjectProposalFullWithId(id);

                if (proposalCreated != null)
                {
                    return CreatedAtAction(nameof(GetById), new { id }, ProjectProposalMapper.ToDto(proposalCreated));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); //si hay un error de conflicto, retorno el mensaje de error
            }
        }



        [HttpGet] // criterio 2: obtener proyectos con filtros AND
        public async Task<IActionResult> GetProjects([FromQuery] ProjectQueryRequestDto filter)
        {
            var validationResult = await _queryValidator.ValidateAsync(filter); //valido los filtros

            if (!validationResult.IsValid)
                return ValidationHelper.FromFluentValidationResult(validationResult);

            var query = _queryService.GetProjectProposalQueryable(); //obtengo la query de proyectos (IQueryable<ProjectProposal>)

            var filteredQuery = await ProjectProposalAsyncFilter.ApplyFiltersAsync(query, filter, _userService);   //aplico los filtros a la query de proyectos

            var result = await filteredQuery.Select(p => ProjectQueryMapper.ToDto(p)).ToListAsync();  

            return Ok(result); 
        }




        [HttpPatch("{id}/decision")] //criterio 3: tomar decision 
        public async Task<IActionResult> MakeDecision(Guid id, [FromBody] DecisionStepRequestDto decision)
        {
            var validationResult = await _decisionValidator.ValidateAsync(decision); //valido la decision
            if (!validationResult.IsValid)
                return ValidationHelper.FromFluentValidationResult(validationResult);

            var projectProposal =  await _queryService.GetProjectProposalByIdAsync(id);
            if (projectProposal == null)
            {
                return NotFound(new { message = "Proyecto no encontrado" });
            }

            var success = await _approveWorkflowService.ApproveStepAsync(decision.Id, decision.User, decision.Status , decision.Observation);

            if (!success)
            {
                return Conflict(new { message = "El proyecto ya no se encuentra en un estado que permite modificaciones." });
            }

            var proposal = await _queryService.GetProjectProposalFullWithId(id);

            return Ok(ProjectProposalMapper.ToDto(proposal));
        }




        [HttpGet("{id}")] //criterio 4: obtener proyecto por id
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!Guid.TryParse(id, out var guid) || guid == Guid.Empty) //valido el guid 
            {
                return BadRequest(new { message = "Id inválido." });
            }

            var proposal = await _queryService.GetProjectProposalFullWithId(guid);

            if (proposal == null)
                return NotFound(new { message = "Proyecto no encontrado" });

            return Ok(ProjectProposalMapper.ToDto(proposal)); 
        }




        [HttpPatch("{id}")] //criterio 5: editar proyecto (solo si está en estado de observación)
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateProjectProposalRequestDto dto)
        {
            var validationResult = await _updateProjectProposalValidator.ValidateAsync(dto); //valido la decision
            if (!validationResult.IsValid)
                return ValidationHelper.FromFluentValidationResult(validationResult);

            if (!Guid.TryParse(id, out var guid) || guid == Guid.Empty)  
            {
                return BadRequest(new { message = "El ID del proyecto es inválido." });
            }

            var projectProposal = await _queryService.GetProjectProposalFullWithId(guid);
            if (projectProposal == null)
            {
                return NotFound(new { message = "No se encontró un proyecto con ese ID." });
            }

            try
            {
                var updatedProposal = await _updaterService.UpdateProjectProposalAsync(guid, dto.title, dto.description, dto.duration);
                if (updatedProposal == null)
                {
                    return NotFound(new { message = "Proyecto no encontrado para actualizar" });
                }
                return Ok(ProjectProposalMapper.ToDto(updatedProposal)); 
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}

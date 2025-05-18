using AprobacionProyectos.Application.DTOs.EntitiesDTOs;
using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Application.DTOs.Response;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AprobacionProyectos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectProposalCreatorService _creatorService;
        private readonly IProjectProposalQueryService _queryService;
        private readonly IUserService _userService;

        public ProjectController(
            IProjectProposalCreatorService creatorService,
            IProjectProposalQueryService repository,
            IUserService userService)
        {
            _creatorService = creatorService;
            _queryService = repository;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectProposalRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new
                {
                    message = "Datos del proyecto inválidos",
                    errors = errores
                });
            }
            // Validación: no repetir títulos
            var existing = await _queryService.GetProjectProposalByTitle(dto.Title);
            if (existing != null)
            {
                return Conflict(new { message = "Ya existe un proyecto con ese título." });
            }

            var proposal = new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                EstimatedAmount = dto.Amount, 
                EstimatedDuration = dto.Duration,
                AreaId = dto.Area,
                CreatedBy = await _userService.GetUserByIdAsync(dto.User),
                TypeId = dto.Type,

            };

            var id = await _creatorService.CreateProjectProposalAsync(proposal);

            var proposalCreated = await _queryService.GetProjectProposalFullWithId(id);

            if (proposalCreated != null)
            {
                var proposalDto = new ProjectProposalResponseDto
                {
                    Id = proposalCreated.Id,
                    Title = proposalCreated.Title,
                    Description = proposalCreated.Description,
                    Amount = proposalCreated.EstimatedAmount,
                    Duration = proposalCreated.EstimatedDuration,
                    Area = new AreaDTO { Id = proposalCreated.Area.Id, Name = proposalCreated.Area.Name },

                    Status = new StatusDTO { Id = proposalCreated.Status.Id, Name = proposalCreated.Status.Name }, 

                    Type = new TypeDTO { Id = proposalCreated.Type.Id, Name = proposalCreated.Type.Name },
                    User = new UserDTO
                    {
                        Id = proposalCreated.CreatedBy.Id,
                        Name = proposalCreated.CreatedBy.Name,
                        Email = proposalCreated.CreatedBy.Email,
                        Role = new ApproverRoleDTO { 
                            Id = proposalCreated.CreatedBy.ApproverRole.Id, 
                            Name = proposalCreated.CreatedBy.ApproverRole.Name },
                    },

                    Steps = proposalCreated.ApprovalSteps.Select(step => new ApprovalStepDTO
                    {
                        Id = step.Id,
                        StepOrder = step.StepOrder,
                        DecisionDate = step.DecisionDate,
                        Observations = step.Observations,

                        ApproverUser = new ApproverUserDTO
                        {
                            Id = step.ApproverUser?.Id,  
                            Name = step.ApproverUser?.Name,  
                            Email = step.ApproverUser?.Email,
                            Role = new ApproverRoleDTO
                            {
                                Id = step.ApproverUser?.ApproverRole.Id, 
                                Name = step.ApproverUser?.ApproverRole.Name
                            }
                        },
                        ApproverRole = new ApproverRoleDTO
                        {
                            Id = step.ApproverRole.Id,
                            Name = step.ApproverRole.Name
                        },
                        Status = new StatusDTO
                        {
                            Id = step.Status.Id,
                            Name = step.Status.Name
                        }
                    }).ToList()
                };
                return CreatedAtAction(nameof(GetById), new { id }, proposalDto);
            }
            else
            {
                return NotFound();
            }       
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var project = await _queryService.GetProjectProposalFullWithId(id);



            if (project == null)
                return NotFound();

            return Ok(project);
        }
    }
}

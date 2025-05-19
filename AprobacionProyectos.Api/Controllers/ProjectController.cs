using AprobacionProyectos.Application.DTOs.EntitiesDTOs;
using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Application.DTOs.Response;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Application.Validators;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;
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

        public ProjectController(
            IProjectProposalCreatorService creatorService,
            IProjectProposalQueryService queryService,
            IUserService userService,
            IApprovalWorkflowService approveWorkflowService)
        {
            _creatorService = creatorService;
            _queryService = queryService;
            _userService = userService;
            _approveWorkflowService = approveWorkflowService;
        }

        [HttpPost] //criterio 1
        public async Task<IActionResult> Create([FromBody] CreateProjectProposalRequestDto dto) //falta mejorar validacion
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
                    Area = new AreaDto { Id = proposalCreated.Area.Id, Name = proposalCreated.Area.Name },

                    Status = new StatusDto { Id = proposalCreated.Status.Id, Name = proposalCreated.Status.Name },

                    Type = new TypeDto { Id = proposalCreated.Type.Id, Name = proposalCreated.Type.Name },
                    User = new UserDto
                    {
                        Id = proposalCreated.CreatedBy.Id,
                        Name = proposalCreated.CreatedBy.Name,
                        Email = proposalCreated.CreatedBy.Email,
                        Role = new ApproverRoleDto
                        {
                            Id = proposalCreated.CreatedBy.ApproverRole.Id,
                            Name = proposalCreated.CreatedBy.ApproverRole.Name
                        },
                    },

                    Steps = proposalCreated.ApprovalSteps.Select(step => new ApprovalStepDto
                    {
                        Id = step.Id,
                        StepOrder = step.StepOrder,
                        DecisionDate = step.DecisionDate,
                        Observations = step.Observations,

                        ApproverUser = new ApproverUserDto
                        {
                            Id = step.ApproverUser?.Id,
                            Name = step.ApproverUser?.Name,
                            Email = step.ApproverUser?.Email,
                            Role = new ApproverRoleDto
                            {
                                Id = step.ApproverUser?.ApproverRole.Id,
                                Name = step.ApproverUser?.ApproverRole.Name
                            }
                        },
                        ApproverRole = new ApproverRoleDto
                        {
                            Id = step.ApproverRole.Id,
                            Name = step.ApproverRole.Name
                        },
                        Status = new StatusDto
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

        [HttpGet] // criterio 2
        public async Task<IActionResult> GetProjects(
            [FromQuery] string? title,
            [FromQuery] int? status,
            [FromQuery] int? applicant,
            [FromQuery] int? approvalUser)  //falta la validacion para que se respete el tipado de c/parámetro
        {
            var validationErrors = ProjectQueryValidator.Validate(title, status, applicant, approvalUser);
            if (validationErrors.Any())
            {
                return BadRequest(new { message = validationErrors });
            }

            var query = _queryService.GetProjectProposalQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(p => p.Title.ToLower().Contains(title.ToLower()));
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status.Id == status.Value);
            }

            if (applicant.HasValue)
            {
                query = query.Where(p => p.CreatedById == applicant.Value);
            }

            if (approvalUser.HasValue)
            {
                query = query.Where(p => p.ApprovalSteps.Any(s => s.ApproverUserId == approvalUser.Value));
            }

            var result = await query.Select(p => new ProjectGetResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Amount = p.EstimatedAmount,
                Duration = p.EstimatedDuration,
                Area = p.Area.Name,
                Status = p.Status.Name,
                Type = p.Type.Name
            }).ToListAsync();

            return Ok(result);

        }

        [HttpPost("{id}/decision")] //criterio 3
        public async Task<IActionResult> MakeDecision(Guid id, [FromBody] DecisionStepRequestDto decision)
        {
            if (decision == null || decision.User <= 0 || !(decision.Status == 2 || decision.Status == 3 || decision.Status == 4))
            {
                return BadRequest(new { message = "Datos de decisión inválidos" });
            }

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

            var result = new ProjectProposalResponseDto
            {
                Id = proposal.Id,
                Title = proposal.Title,
                Description = proposal.Description,
                Amount = proposal.EstimatedAmount,
                Duration = proposal.EstimatedDuration,
                Area = new AreaDto { Id = proposal.Area.Id, Name = proposal.Area.Name },

                Status = new StatusDto { Id = proposal.Status.Id, Name = proposal.Status.Name },

                Type = new TypeDto { Id = proposal.Type.Id, Name = proposal.Type.Name },
                User = new UserDto
                {
                    Id = proposal.CreatedBy.Id,
                    Name = proposal.CreatedBy.Name,
                    Email = proposal.CreatedBy.Email,
                    Role = new ApproverRoleDto
                    {
                        Id = proposal.CreatedBy.ApproverRole.Id,
                        Name = proposal.CreatedBy.ApproverRole.Name
                    },
                },

                Steps = proposal.ApprovalSteps.Select(step => new ApprovalStepDto
                {
                    Id = step.Id,
                    StepOrder = step.StepOrder,
                    DecisionDate = step.DecisionDate,
                    Observations = step.Observations,

                    ApproverUser = new ApproverUserDto
                    {
                        Id = step.ApproverUser?.Id,
                        Name = step.ApproverUser?.Name,
                        Email = step.ApproverUser?.Email,
                        Role = new ApproverRoleDto
                        {
                            Id = step.ApproverUser?.ApproverRole.Id,
                            Name = step.ApproverUser?.ApproverRole.Name
                        }
                    },
                    ApproverRole = new ApproverRoleDto
                    {
                        Id = step.ApproverRole.Id,
                        Name = step.ApproverRole.Name
                    },
                    Status = new StatusDto
                    {
                        Id = step.Status.Id,
                        Name = step.Status.Name
                    }
                }).ToList()
            };
            return Ok(result);
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest(new { message = "Id inválido" });
            }

            var proposal = await _queryService.GetProjectProposalFullWithId(guid);

            if (proposal == null)
                return NotFound(new { message = "Proyecto no encontrado" });

            var result = new ProjectProposalResponseDto
            {
                Id = proposal.Id,
                Title = proposal.Title,
                Description = proposal.Description,
                Amount = proposal.EstimatedAmount,
                Duration = proposal.EstimatedDuration,
                Area = new AreaDto { Id = proposal.Area.Id, Name = proposal.Area.Name },

                Status = new StatusDto { Id = proposal.Status.Id, Name = proposal.Status.Name },

                Type = new TypeDto { Id = proposal.Type.Id, Name = proposal.Type.Name },
                User = new UserDto
                {
                    Id = proposal.CreatedBy.Id,
                    Name = proposal.CreatedBy.Name,
                    Email = proposal.CreatedBy.Email,
                    Role = new ApproverRoleDto
                    {
                        Id = proposal.CreatedBy.ApproverRole.Id,
                        Name = proposal.CreatedBy.ApproverRole.Name
                    },
                },

                Steps = proposal.ApprovalSteps.Select(step => new ApprovalStepDto
                {
                    Id = step.Id,
                    StepOrder = step.StepOrder,
                    DecisionDate = step.DecisionDate,
                    Observations = step.Observations,

                    ApproverUser = new ApproverUserDto
                    {
                        Id = step.ApproverUser?.Id,
                        Name = step.ApproverUser?.Name,
                        Email = step.ApproverUser?.Email,
                        Role = new ApproverRoleDto
                        {
                            Id = step.ApproverUser?.ApproverRole.Id,
                            Name = step.ApproverUser?.ApproverRole.Name
                        }
                    },
                    ApproverRole = new ApproverRoleDto
                    {
                        Id = step.ApproverRole.Id,
                        Name = step.ApproverRole.Name
                    },
                    Status = new StatusDto
                    {
                        Id = step.Status.Id,
                        Name = step.Status.Name
                    }
                }).ToList()
            };
            return Ok(result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.EntitiesDTOs;
using AprobacionProyectos.Application.DTOs.Response;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Mappers
{
    public static class ProjectProposalMapper
    {
        public static ProjectProposalResponseDto ToDto(ProjectProposal proposal)
        {
            return new ProjectProposalResponseDto
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
                    }
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
        }
    }
}

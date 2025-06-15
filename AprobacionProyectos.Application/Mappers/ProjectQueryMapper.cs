using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.Response;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Mappers
{
    public static class ProjectQueryMapper
    {
        public static ProjectGetResponseDto ToDto(ProjectProposal p)
        {
            return new ProjectGetResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Amount = p.EstimatedAmount,
                Duration = p.EstimatedDuration,
                CreatedAt = p.CreatedAt,
                Area = p.Area.Name,
                Status = p.Status.Name,
                Type = p.Type.Name
            };
        }
    }
}

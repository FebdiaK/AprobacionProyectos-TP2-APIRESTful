using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.EntitiesDTOs;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.DTOs.Response
{
    public class ProjectProposalResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public int Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public AreaDto Area { get; set; } = null!;

        public StatusDto Status { get; set; } = null!;
        public TypeDto Type { get; set; } = null!;
        public UserDto User { get; set; } = null!; 
        public List<ApprovalStepDto> Steps { get; set; } = new(); 

    }
}

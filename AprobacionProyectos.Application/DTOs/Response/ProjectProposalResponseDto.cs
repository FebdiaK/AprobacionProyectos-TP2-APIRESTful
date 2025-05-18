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
        public AreaDTO Area { get; set; } = null!;

        public StatusDTO Status { get; set; } = null!;
        public TypeDTO Type { get; set; } = null!;
        public UserDTO User { get; set; } = null!; 
        public List<ApprovalStepDTO> Steps { get; set; } = new(); 

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.EntitiesDTOs;

namespace AprobacionProyectos.Application.DTOs.Response
{
    public class ProjectGetResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public int Duration { get; set; }
        public string Area { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}

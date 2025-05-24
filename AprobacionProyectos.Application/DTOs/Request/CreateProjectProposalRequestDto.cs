using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.DTOs.Request
{
    public class CreateProjectProposalRequestDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }     
        public decimal Amount { get; set; }
        public int Duration { get; set; }    
        public int Area { get; set; }
        public int User { get; set; }
        public int Type { get; set; }

    }
}

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
        [Required(ErrorMessage = "El título es obligatorio")]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Amount { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser mayor a 0")]
        public int Duration { get; set; }
        
        [Required(ErrorMessage = "El area es obligatorio")]
        public int Area { get; set; }
        
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int User { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio")]
        public int Type { get; set; }
    }

}

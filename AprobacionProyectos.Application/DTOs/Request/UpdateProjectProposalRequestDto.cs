using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.DTOs.Request
{
    public class UpdateProjectProposalRequestDto
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nuevo título debe tener entre 3 y 200 caracteres")]
        public string title { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "La nueva descripción debe tener entre 10 y 1000 caracteres")]
        public string description { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser un número mayor a cero")] 
        public int duration { get; set; }
    }
}

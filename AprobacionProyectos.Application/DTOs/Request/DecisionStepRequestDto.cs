using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.DTOs.Request
{
    public class DecisionStepRequestDto
    {
        public long Id { get; set; } // ID del paso de aprobación (tengo que ver si no conviene que en vez del id del paso de aprobación se pasa el stepOrder)
        public int User { get; set; } // el user que decide
        public int Status { get; set; } // 1: Pendiente, 2: Aprobado, 3:Rechazado , 4: Observado
        public string? Observation { get; set; } // observaciones opcionales
    }
}

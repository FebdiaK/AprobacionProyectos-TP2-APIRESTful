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
        public string title { get; set; }
        public string description { get; set; }
        public int duration { get; set; }
    }
}

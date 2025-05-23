using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.DTOs.Request
{
    public class ProjectQueryRequestDto
    {
        public string? title { get; set; }
        public int? status { get; set; }
        public int? applicant { get; set; }
        public int? approvalUser { get; set; }
    }

}

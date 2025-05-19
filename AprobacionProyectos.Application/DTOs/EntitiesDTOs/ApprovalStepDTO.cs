using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.DTOs.EntitiesDTOs
{
    public class ApprovalStepDto
    {
        public long Id { get; set; }
        public int StepOrder { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Observations { get; set; }

        public ApproverUserDto ApproverUser { get; set; } = null!;
        public ApproverRoleDto ApproverRole { get; set; } = null!;
        public StatusDto Status { get; set; } = null!;
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.DTOs.EntitiesDTOs
{
    public class ApprovalStepDTO
    {
        public long Id { get; set; }
        public int StepOrder { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Observations { get; set; }

        public ApproverUserDTO ApproverUser { get; set; } = null!;
        public ApproverRoleDTO ApproverRole { get; set; } = null!;
        public StatusDTO Status { get; set; } = null!;
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.DTOs.Request
{
    public class CreateProjectProposalDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal EstimatedAmount { get; set; }
        public int EstimatedDuration { get; set; }
        public int AreaId { get; set; }
        public int TypeId { get; set; }
        public int CreatedById { get; set; }
    }

}

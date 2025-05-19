using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.DTOs.EntitiesDTOs
{
    public class ApproverUserDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public ApproverRoleDto? Role { get; set; } = null!;
    }
}

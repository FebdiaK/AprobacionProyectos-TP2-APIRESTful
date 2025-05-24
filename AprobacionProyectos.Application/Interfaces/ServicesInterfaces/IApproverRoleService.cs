using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces.ServicesInterfaces
{
    public interface IApproverRoleService
    {
        Task<List<ApproverRole>> GetAllApproverRolesAsync();
        Task<bool> ExistsAsync(int id);
    }
}

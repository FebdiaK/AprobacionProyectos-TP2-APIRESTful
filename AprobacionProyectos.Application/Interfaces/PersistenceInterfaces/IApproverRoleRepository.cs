using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Application.Interfaces.PersistenceInterfaces
{
    public interface IApproverRoleRepository
    {
        Task<List<ApproverRole>> GetAllAsync();
        Task<ApproverRole> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

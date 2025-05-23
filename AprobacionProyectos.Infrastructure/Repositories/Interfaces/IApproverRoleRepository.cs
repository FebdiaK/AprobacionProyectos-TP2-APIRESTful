using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Infrastructure.Repositories.Interfaces
{
    public interface IApproverRoleRepository
    {
        Task<List<ApproverRole>> GetAllAsync();
        Task<ApproverRole> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

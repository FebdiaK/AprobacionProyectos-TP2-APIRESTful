using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;
using Microsoft.EntityFrameworkCore; 

namespace AprobacionProyectos.Application.Interfaces.PersistenceInterfaces 
{ 
    public interface IApprovalStatusRepository
    {
        Task<List<ApprovalStatus>> GetAllAsync();
        Task<ApprovalStatus> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces.PersistenceInterfaces
{
    public interface IApprovalRuleRepository
    {
        Task<List<ApprovalRule>> GetAllAsync();
    }
}

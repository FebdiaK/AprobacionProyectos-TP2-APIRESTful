using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Application.Interfaces.ServicesInterfaces
{
    public interface IApprovalStatusService
    {
        Task<ApprovalStatus> GetApprovalStatusByIdAsync(int id);

        Task<List<ApprovalStatus>> GetAllApprovalStatusesAsync();

        Task<bool> ExistsAsync(int id);

    }
}

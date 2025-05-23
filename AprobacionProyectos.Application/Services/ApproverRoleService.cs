using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Implementations;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;

namespace AprobacionProyectos.Application.Services
{
    public class ApproverRoleService : IApproverRoleService 
    {
        private readonly IApproverRoleRepository _approverRoleRepository; 
        public ApproverRoleService(IApproverRoleRepository approverRoleRepository)
        {
            _approverRoleRepository = approverRoleRepository;
        }

        public async Task<List<ApproverRole>> GetAllApproverRolesAsync()
        {
            return await _approverRoleRepository.GetAllAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _approverRoleRepository.ExistsAsync(id);
        }
    }
}

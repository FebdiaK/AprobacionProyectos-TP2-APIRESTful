using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces.ServicesInterfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Application.Interfaces.PersistenceInterfaces;

namespace AprobacionProyectos.Application.Services
{
    public class ApprovalStatusService : IApprovalStatusService
    {
        private readonly IApprovalStatusRepository _aprrovalStatusRepository;
        public ApprovalStatusService(IApprovalStatusRepository aprrovalStatusRepository)
        {
            _aprrovalStatusRepository = aprrovalStatusRepository;
        }
        public async Task<ApprovalStatus> GetApprovalStatusByIdAsync(int id)
        {
            return await _aprrovalStatusRepository.GetByIdAsync(id);
        }

        public async Task<List<ApprovalStatus>> GetAllApprovalStatusesAsync()
        {
            return await _aprrovalStatusRepository.GetAllAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _aprrovalStatusRepository.ExistsAsync(id);
        }
    }
}

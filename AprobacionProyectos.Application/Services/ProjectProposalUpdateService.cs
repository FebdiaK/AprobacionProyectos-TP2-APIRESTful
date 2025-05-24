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
    public class ProjectProposalUpdateService : IProjectProposalUpdateService
    {
        private readonly IProjectProposalRepository _proposalRepository;

        public ProjectProposalUpdateService(IProjectProposalRepository proposalRepository)
        {
            _proposalRepository = proposalRepository;
        }

        public async Task<ProjectProposal?> UpdateProjectProposalAsync(Guid id, string title, string description, int duration)
        {
            return await _proposalRepository.UpdateAsync( id,  title,  description,  duration);
        }

    }
}

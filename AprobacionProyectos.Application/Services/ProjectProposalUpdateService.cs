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
            var project = await _proposalRepository.GetByIdAsync(id);
            if (project == null)
            {
                return null; //
            }

            if (project.Status.Id != 4)
                throw new InvalidOperationException("El proyecto ya no se encuentra en un estado que permite modificaciones");

            // Validar duplicado título distinto
            var existingProject = await _proposalRepository.GetProjectProposalByTitle(title);
            if (existingProject != null && existingProject.Id != id)
                throw new InvalidOperationException("Ya existe un proyecto con ese título.");

            return await _proposalRepository.UpdateAsync( id,  title,  description,  duration);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces.PersistenceInterfaces
{
    public interface IProjectProposalRepository
    {
        Task<List<ProjectProposal>> GetAllAsync();
        Task<ProjectProposal?> GetByIdAsync(Guid id);
        Task CreateAsync(ProjectProposal projectProposal);
        Task SaveChangesAsync();
        Task<ProjectProposal?> GetProjectProposalByTitle(string title); 
        Task<ProjectProposal?> GetProjectProposalFullWithId(Guid id); 
        IQueryable<ProjectProposal> GetProjectProposalQueryable(); 
        Task<ProjectProposal?> UpdateAsync(Guid id, string title, string description, int duration);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces
{
    public interface IProjectProposalUpdateService
    {
        Task<ProjectProposal?> UpdateProjectProposalAsync(Guid id, string title, string description, int duration);
    }
}

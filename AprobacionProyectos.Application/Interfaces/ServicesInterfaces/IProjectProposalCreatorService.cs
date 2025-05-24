using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces.ServicesInterfaces
{
    public interface IProjectProposalCreatorService
    {
        Task<Guid> CreateProjectProposalAsync(ProjectProposal proposal);

        Task<ProjectProposal?> BuildAsync(CreateProjectProposalRequestDto dto);

    }
}

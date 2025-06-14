using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Application.Interfaces.ServicesInterfaces;
using AprobacionProyectos.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AprobacionProyectos.Application.Helpers
{
    public static class ProjectProposalAsyncFilter
    {
        public static async Task<IQueryable<ProjectProposal>> ApplyFiltersAsync(
            IQueryable<ProjectProposal> query,
            ProjectQueryRequestDto filter,
            IUserService userService
            )
        { 
            if (!string.IsNullOrWhiteSpace(filter.title))
            {
                query = query.Where(p => p.Title.ToLower().Contains(filter.title.ToLower()));
            }

            if (filter.status.HasValue)
            {
                query = query.Where(p => p.Status.Id == filter.status.Value);
            }

            if (filter.applicant.HasValue)
            {
                query = query.Where(p => p.CreatedById == filter.applicant.Value);
            }

            if (filter.approvalUser.HasValue)  //ahora necesito los proyectos donde el user puede participar Y PARTICIPÓ, por ende:
            {
                var user = await userService.GetUserByIdAsync(filter.approvalUser.Value);

                query = query.Where(p =>
                    p.ApprovalSteps.Any(s =>
                        s.ApproverRoleId == user.ApproverRole.Id
                        //&& (s.Status.Id == 1 || s.Status.Id == 4)
                    ));
            }

            return query;
        }
    }
}

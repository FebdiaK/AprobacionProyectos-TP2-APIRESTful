using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Data;
using AprobacionProyectos.Application.Interfaces.PersistenceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Infrastructure.Repositories.Implementations
{
    public class ApproverRoleRepository : IApproverRoleRepository
    {
        private readonly AppDbContext _context;
        public ApproverRoleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ApproverRole>> GetAllAsync()
        {
            return await _context.ApproverRoles.ToListAsync();
        }

        public async Task<ApproverRole> GetByIdAsync(int id)
        {
            var approverRole = await _context.ApproverRoles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == id);

            return approverRole == null ? throw new InvalidOperationException($"No se encontró un ApproverRole con el ID {id}.") : approverRole;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ApproverRoles.AnyAsync(r => r.Id == id);
        }
    }
}

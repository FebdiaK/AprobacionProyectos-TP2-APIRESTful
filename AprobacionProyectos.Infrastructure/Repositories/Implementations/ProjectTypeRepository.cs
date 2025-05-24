using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Infrastructure.Data;
using AprobacionProyectos.Application.Interfaces.PersistenceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Infrastructure.Repositories.Implementations
{
    public class ProjectTypeRepository : IProjectTypeRepository
    {
        private readonly AppDbContext _context;
        public ProjectTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Domain.Entities.ProjectType>> GetAllAsync()
        {
            return await _context.ProjectTypes.ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ProjectTypes.AnyAsync(t => t.Id == id);
        }
    }
}

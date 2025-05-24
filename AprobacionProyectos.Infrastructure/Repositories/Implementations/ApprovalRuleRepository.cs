using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AprobacionProyectos.Application.Interfaces.PersistenceInterfaces;


namespace AprobacionProyectos.Infrastructure.Repositories.Implementations
{
    public class ApprovalRuleRepository : IApprovalRuleRepository
    {
        private readonly AppDbContext _context;
        public ApprovalRuleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ApprovalRule>> GetAllAsync()
        {
            return await _context.ApprovalRules.ToListAsync();
        }
    }
}

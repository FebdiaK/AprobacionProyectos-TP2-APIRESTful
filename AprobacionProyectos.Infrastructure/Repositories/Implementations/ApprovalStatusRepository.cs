﻿using System;
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
    public class ApprovalStatusRepository : IApprovalStatusRepository
    {
        private readonly AppDbContext _context; 
        public ApprovalStatusRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ApprovalStatus>> GetAllAsync()
        {
            return await _context.ApprovalStatuses.ToListAsync();
        }

        public async Task<ApprovalStatus> GetByIdAsync(int id)
        {
            return await _context.ApprovalStatuses.FindAsync(id); 
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ApprovalStatuses.AnyAsync(s => s.Id == id);
        }
    }
}

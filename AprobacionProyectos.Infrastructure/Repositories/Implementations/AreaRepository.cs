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
    public class AreaRepository : IAreaRepository
    {
        private readonly AppDbContext _context;

        public AreaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Area>> GetAllAsync()
        { 
            return await _context.Areas.ToListAsync(); 
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Areas.AnyAsync(a => a.Id == id);
        }
    }
}
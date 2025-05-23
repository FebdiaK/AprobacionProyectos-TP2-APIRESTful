﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id); 
        Task<List<User>> GetAllAsync();
        Task CreateAsync(User user);
        Task<bool> ExistsAsync(int id);
    }
}

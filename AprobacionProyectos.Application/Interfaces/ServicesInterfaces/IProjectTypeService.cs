﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces.ServicesInterfaces
{
    public interface IProjectTypeService
    {
        Task<List<ProjectType>> GetAllProjectTypesAsync();
        Task<bool> ExistsAsync(int id);
    }
}

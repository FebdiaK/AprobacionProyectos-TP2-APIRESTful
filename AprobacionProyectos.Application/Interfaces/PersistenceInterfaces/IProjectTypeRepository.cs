using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces.PersistenceInterfaces
{
    public interface IProjectTypeRepository
    {
        Task<List<ProjectType>> GetAllAsync();

        Task<bool> ExistsAsync(int id);
    }
}

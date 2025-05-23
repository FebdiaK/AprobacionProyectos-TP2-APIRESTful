using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Implementations;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;

namespace AprobacionProyectos.Application.Services
{
    public class ProjectTypeService : IProjectTypeService
    {
        readonly IProjectTypeRepository _projectTypeRepository; 

        public ProjectTypeService(IProjectTypeRepository projectTypeRepository)
        {
            _projectTypeRepository = projectTypeRepository;
        }
        public Task<List<ProjectType>> GetAllProjectTypesAsync()
        {
            return _projectTypeRepository.GetAllAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _projectTypeRepository.ExistsAsync(id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces.ServicesInterfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Application.Interfaces.PersistenceInterfaces;

namespace AprobacionProyectos.Application.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        public AreaService(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }
        public async Task<List<Area>> GetAllAreasAsync()
        {
            return await _areaRepository.GetAllAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _areaRepository.ExistsAsync(id);
        }
    }
}

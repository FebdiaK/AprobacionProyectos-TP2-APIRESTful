using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.Interfaces.PersistenceInterfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}

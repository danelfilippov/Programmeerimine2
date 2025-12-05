using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface ITournamentsRepository
    {
        Task<Tournament> GetByIdAsync(int id);
        Task SaveAsync(Tournament entity);
        Task DeleteAsync(Tournament entity);
    }
}

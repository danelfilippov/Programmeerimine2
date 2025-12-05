using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface ITeamsRepository
    {
        Task<Team> GetByIdAsync(int id);
        Task SaveAsync(Team entity);
        Task DeleteAsync(Team entity);
    }
}

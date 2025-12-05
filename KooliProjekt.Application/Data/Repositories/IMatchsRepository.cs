using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IMatchsRepository
    {
        Task<Match> GetByIdAsync(int id);
        Task SaveAsync(Match entity);
        Task DeleteAsync(Match entity);
    }
}

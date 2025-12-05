using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface ILeaderboardsRepository
    {
        Task<Leaderboard> GetByIdAsync(int id);
        Task SaveAsync(Leaderboard list);
        Task DeleteAsync(Leaderboard entity);
    }
}

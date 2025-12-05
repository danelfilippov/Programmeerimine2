using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class LeaderboardsRepository : BaseRepository<Leaderboard>, ILeaderboardsRepository
    {
        public LeaderboardsRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<Leaderboard> GetByIdAsync(int id)
        {
            return await DbContext
                .Leaderboards
                .Where(list => list.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}

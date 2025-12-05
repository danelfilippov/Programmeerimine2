using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class MatchsRepository : BaseRepository<Match>, IMatchsRepository
    {
        public MatchsRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<Match> GetByIdAsync(int id)
        {
            return await DbContext
                .Matchs
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}

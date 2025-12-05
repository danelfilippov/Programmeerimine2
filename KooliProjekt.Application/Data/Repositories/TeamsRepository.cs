using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class TeamsRepository : BaseRepository<Team>, ITeamsRepository
    {
        public TeamsRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<Team> GetByIdAsync(int id)
        {
            return await DbContext
                .Teams
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}

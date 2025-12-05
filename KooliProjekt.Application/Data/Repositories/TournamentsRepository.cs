using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class TournamentsRepository : BaseRepository<Tournament>, ITournamentsRepository
    {
        public TournamentsRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<Tournament> GetByIdAsync(int id)
        {
            return await DbContext
                .tournaments
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}

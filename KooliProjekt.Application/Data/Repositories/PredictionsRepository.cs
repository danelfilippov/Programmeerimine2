using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class PredictionsRepository : BaseRepository<Prediction>, IPredictionsRepository
    {
        public PredictionsRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<Prediction> GetByIdAsync(int id)
        {
            return await DbContext
                .Predictions
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}

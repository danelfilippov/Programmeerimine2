using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public UsersRepository(ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await DbContext
                .Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}

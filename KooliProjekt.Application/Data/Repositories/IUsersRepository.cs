using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetByIdAsync(int id);
        Task SaveAsync(User entity);
        Task DeleteAsync(User entity);
    }
}

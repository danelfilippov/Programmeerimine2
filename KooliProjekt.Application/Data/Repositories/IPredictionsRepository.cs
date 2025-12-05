using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IPredictionsRepository
    {
        Task<Prediction> GetByIdAsync(int id);
        Task SaveAsync(Prediction entity);
        Task DeleteAsync(Prediction entity);
    }
}

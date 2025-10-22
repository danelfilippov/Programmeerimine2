using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.TodoLists
{
    public class PredictionsQueryHandler : IRequestHandler<PredictionsQuery, OperationResult<IList<Prediction>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public PredictionsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Prediction>>> Handle(PredictionsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Prediction>>();
            result.Value = await _dbContext
                .Predictions
                .OrderBy(list => list.Id)
                .ToListAsync();

            return result;
        }
    }
}

using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Predictions
{
    public class GetPredictionsQueryHandler : IRequestHandler<GetPredictionsQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetPredictionsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetPredictionsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Predictions
                .Include(List => List.Id)
                .Where(List => List.Id == request.Id)
                .Select(List => new
                {
                    Id = List.Id,
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
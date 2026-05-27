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

namespace KooliProjekt.Application.Features.Predictions
{
    public class PredictionsQueryHandler : IRequestHandler<PredictionsQuery, OperationResult<PagedResult<Prediction>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public PredictionsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Prediction>>> Handle(PredictionsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Prediction>>();

            IQueryable<Prediction> query = _dbContext.Predictions;

            // Apply search filter if Title is provided
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                var searchTerm = request.Title.ToLower();
                query = query.Where(p => p.Title.ToLower().Contains(searchTerm) || p.Status.ToLower().Contains(searchTerm));
            }

            result.Value = await query
                .OrderBy(list => list.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}

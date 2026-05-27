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

namespace KooliProjekt.Application.Features.Matchs
{
    public class MatchsQueryHandler : IRequestHandler<MatchsQuery, OperationResult<PagedResult<Match>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public MatchsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Match>>> Handle(MatchsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Match>>();

            IQueryable<Match> query = _dbContext.Matchs;

            // Apply search filter if Title is provided
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                var searchTerm = request.Title.ToLower();
                query = query.Where(m => m.Title.ToLower().Contains(searchTerm) || m.Description.ToLower().Contains(searchTerm));
            }

            result.Value = await query
                .OrderBy(list => list.StartTime)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}

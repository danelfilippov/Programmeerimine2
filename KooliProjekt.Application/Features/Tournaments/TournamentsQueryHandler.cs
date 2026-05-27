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

namespace KooliProjekt.Application.Features.Tournaments
{
    public class TournamentsQueryHandler : IRequestHandler<TournamentsQuery, OperationResult<PagedResult<Tournament>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public TournamentsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Tournament>>> Handle(TournamentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Tournament>>();

            IQueryable<Tournament> query = _dbContext.tournaments;

            // Apply search filter if Title is provided
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                var searchTerm = request.Title.ToLower();
                query = query.Where(t => t.Title.ToLower().Contains(searchTerm) || t.Name.ToLower().Contains(searchTerm));
            }

            result.Value = await query
                .OrderBy(list => list.Name)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}

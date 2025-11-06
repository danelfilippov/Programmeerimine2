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
            result.Value = await _dbContext
                .tournaments
                .OrderBy(list => list.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}

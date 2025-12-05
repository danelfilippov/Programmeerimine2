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

namespace KooliProjekt.Application.Features.Teams
{
    public class TeamsQueryHandler : IRequestHandler<TeamsQuery, OperationResult<PagedResult<Team>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public TeamsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Team>>> Handle(TeamsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Team>>();
            result.Value = await _dbContext
                .Teams
                .OrderBy(list => list.Id)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}

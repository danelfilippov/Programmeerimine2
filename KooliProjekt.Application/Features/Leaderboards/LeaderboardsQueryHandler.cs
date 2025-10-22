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
    public class LeaderboardsQueryHandler : IRequestHandler<LeaderboardsQuery, OperationResult<IList<Leaderboard>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public LeaderboardsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Leaderboard>>> Handle(LeaderboardsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Leaderboard>>();
            result.Value = await _dbContext
                .Leaderboards
                .OrderBy(list => list.Id)
                .ToListAsync();

            return result;
        }
    }
}

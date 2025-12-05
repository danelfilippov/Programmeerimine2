using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Leaderboards
{
    public class GetLeaderboardsQueryHandler : IRequestHandler<GetLeaderboardsQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetLeaderboardsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetLeaderboardsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Leaderboards
                .Include(list => list.UserId)
                .Where(list => list.Id == request.Id)
                .Select(list => new
                {
                    Id = list.Id,
                    UserId = list.UserId,
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Leaderboards;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Leaderboards
{
    public class SaveLeaderboardsCommandHandler : IRequestHandler<SaveLeaderboardsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveLeaderboardsCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveLeaderboardsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Leaderboard();
            if(request.Id == 0)
            {
                await _dbContext.Leaderboards.AddAsync(list);
            }
            else
            {
                list = await _dbContext.Leaderboards.FindAsync(request.Id);
            }

            list.Id = request.Id;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Leaderboards
{
    public class SaveLeaderboardsCommandHandler : IRequestHandler<SaveLeaderboardsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveLeaderboardsCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveLeaderboardsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();
            if(request.Id < 0)
            {
                result.AddPropertyError(nameof(request.Id), "Id cannot be negative");
                return result;
            }
            
            var list = new Leaderboard();

            if(request.Id == 0)
            {
                await _dbContext.Leaderboards.AddAsync(list);
            }
            else
            {
                list = await _dbContext.Leaderboards.FindAsync(request.Id);
                if (list == null)
                {
                    result.AddError("Cannot find list with id " + request.Id);
                    return result;
                }
            }

            list.Title = request.Title;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
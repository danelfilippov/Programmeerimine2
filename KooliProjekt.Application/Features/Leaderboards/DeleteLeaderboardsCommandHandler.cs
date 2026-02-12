using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.Features.Leaderboards
{
    public class DeleteLeaderboardsCommandHandler : IRequestHandler<DeleteLeaderboardsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteLeaderboardsCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteLeaderboardsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            var list = await _dbContext
                .Leaderboards
                .Include(t => t.Id)
                .FirstOrDefaultAsync(t => t.Id == request.Id);
            
            if(list == null)
            {
                return result;
            }

            _dbContext.Leaderboards.RemoveRange(list);
            _dbContext.Leaderboards.Remove(list);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
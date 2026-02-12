using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Matchs
{
    public class DeleteMatchsCommandHandler : IRequestHandler<DeleteMatchsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteMatchsCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteMatchsCommand request, CancellationToken cancellationToken)
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

            var match = await _dbContext
                .Matchs
                .Include(t => t.Id)
                .FirstOrDefaultAsync(t => t.Id == request.Id);
            
            if(match == null)
            {
                return result;
            }

            _dbContext.Matchs.RemoveRange(match);
            _dbContext.Matchs.Remove(match);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}

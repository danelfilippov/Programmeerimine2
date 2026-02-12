using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Teams
{
    public class DeleteTeamsCommandHandler : IRequestHandler<DeleteTeamsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteTeamsCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteTeamsCommand request, CancellationToken cancellationToken)
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

            var team = await _dbContext
                .Teams
                .Include(t => t.Id)
                .FirstOrDefaultAsync(t => t.Id == request.Id);
            
            if(team == null)
            {
                return result;
            }

            _dbContext.Teams.RemoveRange(team);
            _dbContext.Teams.Remove(team);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}

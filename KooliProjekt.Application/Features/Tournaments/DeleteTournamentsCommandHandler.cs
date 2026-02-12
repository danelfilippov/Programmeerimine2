using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class DeleteTournamentsCommandHandler : IRequestHandler<DeleteTournamentsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteTournamentsCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteTournamentsCommand request, CancellationToken cancellationToken)
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

            var tournament = await _dbContext
                .tournaments
                .Include(t => t.Id)
                .FirstOrDefaultAsync(t => t.Id == request.Id);
            
            if(tournament == null)
            {
                return result;
            }

            _dbContext.tournaments.RemoveRange(tournament);
            _dbContext.tournaments.Remove(tournament);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}

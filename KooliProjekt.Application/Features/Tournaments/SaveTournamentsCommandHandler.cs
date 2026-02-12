using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class SaveTournamentsCommandHandler : IRequestHandler<SaveTournamentsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveTournamentsCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTournamentsCommand request, CancellationToken cancellationToken)
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
            
            var tournament = new Tournament();

            if(request.Id == 0)
            {
                await _dbContext.tournaments.AddAsync(tournament);
            }
            else
            {
                tournament = await _dbContext.tournaments.FindAsync(request.Id);
                if (tournament == null)
                {
                    result.AddError("Cannot find tournament with id " + request.Id);
                    return result;
                }
            }

            tournament.Title = request.Title;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
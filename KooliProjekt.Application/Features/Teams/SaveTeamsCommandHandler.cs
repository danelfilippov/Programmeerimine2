using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class SaveTeamsCommandHandler : IRequestHandler<SaveTeamsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveTeamsCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTeamsCommand request, CancellationToken cancellationToken)
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
            
            var team = new Team();

            if(request.Id == 0)
            {
                await _dbContext.Teams.AddAsync(team);
            }
            else
            {
                team = await _dbContext.Teams.FindAsync(request.Id);
                if (team == null)
                {
                    result.AddError("Cannot find team with id " + request.Id);
                    return result;
                }
            }

            team.Title = request.Title;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
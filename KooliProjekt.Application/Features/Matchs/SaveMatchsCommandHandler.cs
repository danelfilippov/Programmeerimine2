using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Matchs
{
    public class SaveMatchsCommandHandler : IRequestHandler<SaveMatchsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveMatchsCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveMatchsCommand request, CancellationToken cancellationToken)
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
            
            var match = new Match();

            if(request.Id == 0)
            {
                await _dbContext.Matchs.AddAsync(match);
            }
            else
            {
                match = await _dbContext.Matchs.FindAsync(request.Id);
                if (match == null)
                {
                    result.AddError("Cannot find match with id " + request.Id);
                    return result;
                }
            }

            match.Title = request.Title;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
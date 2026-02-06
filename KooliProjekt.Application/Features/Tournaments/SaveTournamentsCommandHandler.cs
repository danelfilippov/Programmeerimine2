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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTournamentsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Tournament();
            if(request.Id == 0)
            {
                await _dbContext.tournaments.AddAsync(list);
            }
            else
            {
                list = await _dbContext.tournaments.FindAsync(request.Id);
            }

            list.Id = request.Id;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveMatchsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Match();
            if(request.Id == 0)
            {
                await _dbContext.Matchs.AddAsync(list);
            }
            else
            {
                list = await _dbContext.Matchs.FindAsync(request.Id);
            }

            list.Id = request.Id;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
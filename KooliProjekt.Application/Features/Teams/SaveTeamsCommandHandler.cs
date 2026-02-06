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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTeamsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Team();
            if(request.Id == 0)
            {
                await _dbContext.Teams.AddAsync(list);
            }
            else
            {
                list = await _dbContext.Teams.FindAsync(request.Id);
            }

            list.Id = request.Id;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
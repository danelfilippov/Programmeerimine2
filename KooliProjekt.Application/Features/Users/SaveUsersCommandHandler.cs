using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUsersCommandHandler : IRequestHandler<SaveUsersCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveUsersCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveUsersCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new User();
            if(request.Id == 0)
            {
                await _dbContext.Users.AddAsync(list);
            }
            else
            {
                list = await _dbContext.Users.FindAsync(request.Id);
            }

            list.Id = request.Id;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
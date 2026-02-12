using System;
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
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveUsersCommand request, CancellationToken cancellationToken)
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
            
            var user = new User();

            if(request.Id == 0)
            {
                await _dbContext.Users.AddAsync(user);
            }
            else
            {
                user = await _dbContext.Users.FindAsync(request.Id);
                if (user == null)
                {
                    result.AddError("Cannot find user with id " + request.Id);
                    return result;
                }
            }

            user.Title = request.Title;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
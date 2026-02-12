using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class DeleteUsersCommandHandler : IRequestHandler<DeleteUsersCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteUsersCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteUsersCommand request, CancellationToken cancellationToken)
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

            var user = await _dbContext
                .Users
                .Include(t => t.Id)
                .FirstOrDefaultAsync(t => t.Id == request.Id);
            
            if(user == null)
            {
                return result;
            }

            _dbContext.Users.RemoveRange(user);
            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}

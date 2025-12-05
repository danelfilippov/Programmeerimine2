using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUsersCommandHandler : IRequestHandler<SaveUsersCommand, OperationResult>
    {
        private readonly IUsersRepository _usersRepository;

        public SaveUsersCommandHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<OperationResult> Handle(SaveUsersCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var user = new User();
            if (request.Id != 0)
            {
                user = await _usersRepository.GetByIdAsync(request.Id);
            }

            user.Id = request.Id;

            await _usersRepository.SaveAsync(user);

            return result;
        }
    }
}

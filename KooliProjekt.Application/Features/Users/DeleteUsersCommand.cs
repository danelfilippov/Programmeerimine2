using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class DeleteUsersCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}

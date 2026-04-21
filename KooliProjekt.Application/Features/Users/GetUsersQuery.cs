using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class GetUsersQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}

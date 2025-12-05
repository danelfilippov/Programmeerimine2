using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class GetTeamsQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}

using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Matchs
{
    public class GetMatchsQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}

using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class GetTournamentsQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}

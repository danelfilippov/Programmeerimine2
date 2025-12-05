using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class DeleteTournamentsCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}

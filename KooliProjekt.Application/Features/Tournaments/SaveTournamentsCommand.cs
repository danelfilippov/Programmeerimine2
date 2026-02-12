using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class SaveTournamentsCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}

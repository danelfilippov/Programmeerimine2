using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Matchs

{
    public class SaveMatchsCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

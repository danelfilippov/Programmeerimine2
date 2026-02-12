using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Teams

{
    public class SaveTeamsCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}

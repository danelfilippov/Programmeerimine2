using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.ToDoLists
{
    public class SaveLeaderboardsCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}

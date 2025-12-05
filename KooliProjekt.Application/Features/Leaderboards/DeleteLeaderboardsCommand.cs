using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Leaderboards
{
    public class DeleteLeaderboardsCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}

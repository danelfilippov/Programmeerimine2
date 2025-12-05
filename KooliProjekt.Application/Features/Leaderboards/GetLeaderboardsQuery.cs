using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Leaderboards
{
    public class GetLeaderboardsQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}

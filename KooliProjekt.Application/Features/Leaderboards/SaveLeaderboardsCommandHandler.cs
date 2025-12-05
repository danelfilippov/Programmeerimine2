using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.Leaderboards;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Leaderboards
{
    public class SaveLeaderboardsCommandHandler : IRequestHandler<SaveLeaderboardsCommand, OperationResult>
    {
        private readonly ILeaderboardsRepository _leaderboardsRepository;

        public SaveLeaderboardsCommandHandler(ILeaderboardsRepository leaderboardsRepository)
        {
            _leaderboardsRepository = leaderboardsRepository;
        }

        public async Task<OperationResult> Handle(SaveLeaderboardsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Leaderboard();
            if (request.Id != 0)
            {
                list = await _leaderboardsRepository.GetByIdAsync(request.Id);
            }

            list.Id = request.Id;

            await _leaderboardsRepository.SaveAsync(list);

            return result;
        }
    }
}

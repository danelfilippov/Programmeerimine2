using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class SaveTeamsCommandHandler : IRequestHandler<SaveTeamsCommand, OperationResult>
    {
        private readonly ITeamsRepository _teamsRepository;

        public SaveTeamsCommandHandler(ITeamsRepository teamsRepository)
        {
            _teamsRepository = teamsRepository;
        }

        public async Task<OperationResult> Handle(SaveTeamsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var team = new Team();
            if (request.Id != 0)
            {
                team = await _teamsRepository.GetByIdAsync(request.Id);
            }

            team.Id = request.Id;

            await _teamsRepository.SaveAsync(team);

            return result;
        }
    }
}
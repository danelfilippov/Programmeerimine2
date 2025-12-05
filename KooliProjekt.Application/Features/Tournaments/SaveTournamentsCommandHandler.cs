using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.Leaderboards;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class SaveTournamentsCommandHandler : IRequestHandler<SaveTournamentsCommand, OperationResult>
    {
        private readonly ITournamentsRepository _tournamentsRepository;

        public SaveTournamentsCommandHandler(ITournamentsRepository tournamentsRepository)
        {
            _tournamentsRepository = tournamentsRepository;
        }

        public async Task<OperationResult> Handle(SaveTournamentsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Tournament();
            if (request.Id != 0)
            {
                list = await _tournamentsRepository.GetByIdAsync(request.Id);
            }

            list.Id = request.Id;

            await _tournamentsRepository.SaveAsync(list);

            return result;
        }
    }
}

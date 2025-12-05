using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Matchs
{
    public class SaveMatchsCommandHandler : IRequestHandler<SaveMatchsCommand, OperationResult>
    {
        private readonly IMatchsRepository _matchsRepository;

        public SaveMatchsCommandHandler(IMatchsRepository matchsRepository)
        {
            _matchsRepository = matchsRepository;
        }

        public async Task<OperationResult> Handle(SaveMatchsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var match = new Match();
            if (request.Id != 0)
            {
                match = await _matchsRepository.GetByIdAsync(request.Id);
            }

            match.Id = request.Id;

            await _matchsRepository.SaveAsync(match);

            return result;
        }
    }
}

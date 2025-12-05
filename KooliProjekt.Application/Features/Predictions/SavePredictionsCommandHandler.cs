using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Features.Leaderboards;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class SavePredictionsCommandHandler : IRequestHandler<SavePredictionsCommand, OperationResult>
    {
        private readonly IPredictionsRepository _predictionsRepository;

        public SavePredictionsCommandHandler(IPredictionsRepository predictionsRepository)
        {
            _predictionsRepository = predictionsRepository;
        }

        public async Task<OperationResult> Handle(SavePredictionsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var prediction = new Prediction();
            if (request.Id != 0)
            {
                prediction = await _predictionsRepository.GetByIdAsync(request.Id);
            }

            prediction.Id = request.Id;

            await _predictionsRepository.SaveAsync(prediction);

            return result;
        }
    }
}

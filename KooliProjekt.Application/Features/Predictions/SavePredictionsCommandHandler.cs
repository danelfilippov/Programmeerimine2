using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Predictions
{
    public class SavePredictionsCommandHandler : IRequestHandler<SavePredictionsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SavePredictionsCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SavePredictionsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var prediction = new Prediction();
            if (request.Id == 0)
            {
                await _dbContext.Predictions.AddAsync(prediction, cancellationToken);
            }
            else
            {
                prediction = await _dbContext.Predictions.FindAsync(new object[] { request.Id }, cancellationToken);
                //_dbContext.Predictions.Update(prediction);
            }

            prediction.Id = request.Id;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
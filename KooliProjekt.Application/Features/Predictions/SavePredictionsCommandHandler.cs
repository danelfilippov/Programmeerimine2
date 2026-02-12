using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class SavePredictionsCommandHandler : IRequestHandler<SavePredictionsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SavePredictionsCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SavePredictionsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();
            if(request.Id < 0)
            {
                result.AddPropertyError(nameof(request.Id), "Id cannot be negative");
                return result;
            }
            
            var prediction = new Prediction();

            if(request.Id == 0)
            {
                await _dbContext.Predictions.AddAsync(prediction);
            }
            else
            {
                prediction = await _dbContext.Predictions.FindAsync(request.Id);
                if (prediction == null)
                {
                    result.AddError("Cannot find prediction with id " + request.Id);
                    return result;
                }
            }

            prediction.Title = request.Title;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
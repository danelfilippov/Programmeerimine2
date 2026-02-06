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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SavePredictionsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Prediction();
            if(request.Id == 0)
            {
                await _dbContext.Predictions.AddAsync(list);
            }
            else
            {
                list = await _dbContext.Predictions.FindAsync(request.Id);
            }

            list.Id = request.Id;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
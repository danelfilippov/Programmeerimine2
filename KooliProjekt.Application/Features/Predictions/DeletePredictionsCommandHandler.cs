using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Predictions
{
    public class DeletePredictionsCommandHandler : IRequestHandler<DeletePredictionsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeletePredictionsCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeletePredictionsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            var prediction = await _dbContext
                .Predictions
                .Include(t => t.Id)
                .FirstOrDefaultAsync(t => t.Id == request.Id);
            
            if(prediction == null)
            {
                return result;
            }

            _dbContext.Predictions.RemoveRange(prediction);
            _dbContext.Predictions.Remove(prediction);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}

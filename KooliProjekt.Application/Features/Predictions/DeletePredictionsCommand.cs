using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class DeletePredictionsCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}

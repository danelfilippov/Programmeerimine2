using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class GetPredictionsQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class PredictionsQuery : IRequest<OperationResult<PagedResult<Prediction>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

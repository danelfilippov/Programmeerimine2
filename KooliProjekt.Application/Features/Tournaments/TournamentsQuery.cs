using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TodoLists
{
    public class TournamentsQuery : IRequest<OperationResult<IList<Tournament>>>
    {
    }
}

using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class DeleteTeamsCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}

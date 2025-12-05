using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Matchs
{
    public class DeleteMatchsCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}

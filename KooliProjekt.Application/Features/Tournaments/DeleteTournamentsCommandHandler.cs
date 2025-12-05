using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class DeleteTournamentsCommandHandler : IRequestHandler<DeleteTournamentsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteTournamentsCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteTournamentsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .tournaments
                .Where(t => t.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}

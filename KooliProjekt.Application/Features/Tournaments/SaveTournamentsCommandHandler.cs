using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace KooliProjekt.Application.Features.ToDoLists
{
    public class SaveTournamentsCommandHandler : IRequestHandler<SaveTournamentsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveTournamentsCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTournamentsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Tournament();
            if (request.Id == 0)
            {
                await _dbContext.tournaments.AddAsync(list);
            }
            else
            {
                list = await _dbContext.tournaments.FindAsync(request.Id);
                //_dbContext.ToDoLists.Update(list);
            }

            list.Id = request.Id;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.ToDoLists
{
    public class GetMatchsQueryHandler : IRequestHandler<GetMatchsQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetMatchsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetMatchsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Matchs
                .Include(list => list.Id)
                .Where(list => list.Id == request.Id)
                .Select(list => new
                {
                    Id = list.Id,
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}

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
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class UsersQueryHandler : IRequestHandler<UsersQuery, OperationResult<PagedResult<User>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public UsersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<User>>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<User>>();

            IQueryable<User> query = _dbContext.Users;

            // Apply search filter if Title is provided
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                var searchTerm = request.Title.ToLower();
                query = query.Where(u => u.Title.ToLower().Contains(searchTerm) || u.Name.ToLower().Contains(searchTerm));
            }

            result.Value = await query
                .OrderBy(list => list.Name)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}

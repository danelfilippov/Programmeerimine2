﻿using System;
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

namespace KooliProjekt.Application.Features.TodoLists
{
    public class UsersQueryHandler : IRequestHandler<UsersQuery, OperationResult<IList<User>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public UsersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<User>>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<User>>();
            result.Value = await _dbContext
                .Users
                .OrderBy(list => list.Id)
                .ToListAsync();

            return result;
        }
    }
}

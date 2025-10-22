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
    public class TournamentsQueryHandler : IRequestHandler<TournamentsQuery, OperationResult<IList<Tournament>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public TournamentsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Tournament>>> Handle(TournamentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Tournament>>();
            result.Value = await _dbContext
                .tournaments
                .OrderBy(list => list.Id)
                .ToListAsync();

            return result;
        }
    }
}

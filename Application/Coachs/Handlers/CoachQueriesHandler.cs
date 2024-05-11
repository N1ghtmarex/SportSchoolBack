﻿using Abstractions.CommonModels;
using Application.Coachs.Dtos;
using Application.Coachs.Queries;
using Core.EntityFramework.Features.SearchPagination;
using Core.EntityFramework.Features.SearchPagination.Models;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Coachs.Handlers
{
    internal class CoachQueriesHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, ICoachMapper coachMapper) :
        IRequestHandler<GetCoachsListQuery, PagedResult<CoachListViewModel>>, IRequestHandler<GetCoachQuery, CoachViewModel>
    {
        public async Task<PagedResult<CoachListViewModel>> Handle(GetCoachsListQuery request, CancellationToken cancellationToken)
        {
            var coachQuery = dbContext.Coachs
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Surname)
                .ApplySearch(request, x => x.Name, x => x.Surname);

            var coachsList = await coachQuery
                .ApplyPagination(request)
                .ToListAsync(cancellationToken);

            var result = coachsList.Select(coachMapper.MapToListViewModel);
            return result.AsPagedResult(request, await coachQuery.CountAsync(cancellationToken));
        }

        public Task<CoachViewModel> Handle(GetCoachQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

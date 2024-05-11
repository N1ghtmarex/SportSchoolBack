using Abstractions.CommonModels;
using Application.Sports.Dtos;
using Application.Sports.Queries;
using Core.EntityFramework.Features.SearchPagination;
using Core.EntityFramework.Features.SearchPagination.Models;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Sports.Handlers
{
    internal class SportQueriesHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, ISportMapper sportMapper) :
        IRequestHandler<GetSportsListQuery, PagedResult<SportListViewModel>>, IRequestHandler<GetSportQuery, SportViewModel>
    {
        public async Task<PagedResult<SportListViewModel>> Handle(GetSportsListQuery request, CancellationToken cancellationToken)
        {
            var sportQuery = dbContext.Sports
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ApplySearch(request, x => x.Name);

            var sportsList = await sportQuery
                .ApplyPagination(request)
            .ToListAsync(cancellationToken);

            var result = sportsList.Select(sportMapper.MapToListViewModel);
            return result.AsPagedResult(request, await sportQuery.CountAsync(cancellationToken));
        }

        public Task<SportViewModel> Handle(GetSportQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

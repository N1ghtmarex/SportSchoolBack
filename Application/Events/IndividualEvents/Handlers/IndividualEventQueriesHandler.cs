using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Events.IndividualEvents.Dtos;
using Application.Events.IndividualEvents.Queries;
using Core.EntityFramework.Features.SearchPagination;
using Core.EntityFramework.Features.SearchPagination.Models;
using Core.Exceptions;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Events.IndividualEvents.Handlers
{
    internal class IndividualEventQueriesHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, IIndividualEventMapper individualEventMapper,
        IClientService clientService) :
        IRequestHandler<GetIndividualEventsListQuery, PagedResult<IndividualEventListViewModel>>, IRequestHandler<GetIndividualEventQuery, IndividualEventListViewModel>,
        IRequestHandler<GetClientsIndividualEventsListQuery, PagedResult<IndividualEventListViewModel>>
    {
        public async Task<PagedResult<IndividualEventListViewModel>> Handle(GetIndividualEventsListQuery request, CancellationToken cancellationToken)
        {
            var individualEventQuery = dbContext.IndividualEvents
                .AsNoTracking();

            if (request.Filter.ToLower() == "free")
            {
                individualEventQuery = individualEventQuery.Where(x => x.ClientId == null);
            }
            else if (request.Filter.ToLower() == "reserved")
            {
                individualEventQuery = individualEventQuery.Where(x => x.ClientId != null);
            }
            else if (request.Filter.ToLower() != "all")
            {
                throw new BusinessLogicException(
                    $"Указан неправильный фильтр!");
            }

            if (request.SportId != null)
            {
                individualEventQuery = individualEventQuery
                    .Where(x => x.SportId == request.SportId);
            }

            individualEventQuery = individualEventQuery
                .Include(x => x.Sport)
                .Include(x => x.Coach)
                .Include(x => x.Room)
                .OrderBy(x => x.StartDate)
                .ThenBy(x => x.EndDate)
                .ApplySearch(request, x => x.Sport.Name);

            var individualEventsList = await individualEventQuery
                .ApplyPagination(request)
                .ToListAsync(cancellationToken);

            var result = individualEventsList.Select(individualEventMapper.MapToListViewModel);
            return result.AsPagedResult(request, await individualEventQuery.CountAsync(cancellationToken));
        }

        public async Task<IndividualEventListViewModel> Handle(GetIndividualEventQuery request, CancellationToken cancellationToken)
        {
            var existingEvent = await dbContext.IndividualEvents
                .AsNoTracking()
                .Where(x => x.Id == request.IndividualEventId)
                .Include(x => x.Sport)
                .Include(x => x.Coach)
                .Include(x => x.Room)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new ObjectNotFoundException($"Индивидуальное занятие с идентификатором {request.IndividualEventId} не найдено!");

            return individualEventMapper.MapToListViewModel(existingEvent);
        }

        public async Task<PagedResult<IndividualEventListViewModel>> Handle(GetClientsIndividualEventsListQuery request, CancellationToken cancellationToken)
        {
            var individualEventQuery = dbContext.IndividualEvents
                .AsNoTracking();

            if (contextAccessor.UserRoles.Contains("Coach"))
            {
                var coach = await dbContext.Coachs
                .Where(x => x.ExternalId == Guid.Parse(contextAccessor.IdentityUserId))
                .Include(x => x.Section)
                .SingleOrDefaultAsync(cancellationToken);

                if (coach == null)
                {
                    throw new ObjectNotFoundException(
                        $"Тренер с внешним идентификатором \"{contextAccessor.IdentityUserId}\" не найден!");
                }

                individualEventQuery = individualEventQuery
                    .Where(x => x.CoachId == coach.Id);
            }
            else
            {
                var client = await clientService.GetClientAsync(contextAccessor.IdentityUserId, false, cancellationToken);

                individualEventQuery = individualEventQuery
                    .Where(x => x.ClientId == client.Id);
            }
            
            individualEventQuery = individualEventQuery
                .Include(x => x.Sport)
                .Include(x => x.Coach)
                .Include(x => x.Room)
                .OrderBy(x => x.StartDate)
                .ThenBy(x => x.EndDate)
                .ApplySearch(request, x => x.Sport.Name);

            var individualEventsList = await individualEventQuery
                .ApplyPagination(request)
                .ToListAsync(cancellationToken);

            foreach (var item in individualEventsList)
            {
                item.StartDate = item.StartDate.ToLocalTime();
                item.EndDate = item.EndDate.ToLocalTime();
            }

            var result = individualEventsList.Select(individualEventMapper.MapToListViewModel);
            return result.AsPagedResult(request, await individualEventQuery.CountAsync(cancellationToken));
        }
    }
}

using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Events.SectionEvents.Dtos;
using Application.Events.SectionEvents.Queries;
using Core.EntityFramework.Features.SearchPagination;
using Core.EntityFramework.Features.SearchPagination.Models;
using Core.Exceptions;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Events.SectionEvents.Handlers
{
    internal class SectionEventQueriesHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor,
        ISectionEventMapper sectionEventMapper, IClientService clientService, ISectionService sectionService, ICoachService coachService) :
        IRequestHandler<GetSectionEventsListQuery, PagedResult<SectionEventListViewModel>>, IRequestHandler<GetSectionEventQuery, PagedResult<SectionEventListViewModel>>
    {
        public async Task<PagedResult<SectionEventListViewModel>> Handle(GetSectionEventsListQuery request, CancellationToken cancellationToken)
        {
            var sectionEventQuery = dbContext.SectionEvents
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

                sectionEventQuery = sectionEventQuery.
                    Where(x => coach.Section.Contains(x.Section));
            }
            else
            {
                var client = await dbContext.Clients
                .Where(x => x.ExternalId == Guid.Parse(contextAccessor.IdentityUserId))
                .Include(x => x.Section)
                .SingleOrDefaultAsync(cancellationToken);

                if (client == null)
                {
                    throw new ObjectNotFoundException(
                        $"Пользователь с внешним идентификатором \"{contextAccessor.IdentityUserId}\" не найден!");
                }

                sectionEventQuery = sectionEventQuery.
                    Where(x => client.Section.Contains(x.Section));

            }
            

            sectionEventQuery = sectionEventQuery
                .Include(x => x.Section)
                    .ThenInclude(x => x.Sport)
                .Include(x => x.Section)
                    .ThenInclude(x => x.Room)
                .Include(x => x.Section)
                    .ThenInclude(x => x.Coach)
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .ApplySearch(request, 
                    x => x.DayOfWeek.ToString(), 
                    x => x.StartTime.ToString(), 
                    x => x.EndTime.ToString()
                );

            var sectionEventsList = await sectionEventQuery
                .ApplyPagination(request)
                .ToListAsync(cancellationToken);

            var result = sectionEventsList.Select(sectionEventMapper.MapToListViewModel);
            return result.AsPagedResult(request, await sectionEventQuery.CountAsync(cancellationToken));
        }

        public async Task<PagedResult<SectionEventListViewModel>> Handle(GetSectionEventQuery request, CancellationToken cancellationToken)
        {
            var sectionIds = new List<Guid>();

            if (contextAccessor.UserRoles.Contains("Coach"))
            {
                var client = await clientService.GetClientAsync(contextAccessor.IdentityUserId, true, cancellationToken);

                sectionIds = client.Section
                .Select(x => x.Id)
                .ToList();
            }
            else
            {
                var coach = await coachService.GetCoachAync(contextAccessor.IdentityUserId, cancellationToken);

                sectionIds = coach.Section
                .Select(x => x.Id)
                .ToList();
            }

            var section = await sectionService.GetSectionAsync(request.SectionId, false, false, false, false, cancellationToken);

            if (!sectionIds.Contains(section.Id)) 
            {
                throw new BusinessLogicException(
                    $"Пользователь с внешним идентификатором \"{contextAccessor.IdentityUserId}\" не состоит в секции с идентификатором \"{section.Id}\"!");
            }

            var sectionEventQuery = dbContext.SectionEvents
                .AsNoTracking()
                .Where(x => x.SectionId == section.Id)
                .Include(x => x.Section)
                    .ThenInclude(x => x.Sport)
                .Include(x => x.Section)
                    .ThenInclude(x => x.Room)
                .Include(x => x.Section)
                    .ThenInclude(x => x.Coach)
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .ApplySearch(request,
                    x => x.DayOfWeek.ToString(),
                    x => x.StartTime.ToString(),
                    x => x.EndTime.ToString()
                );

            var sectionEventsList = await sectionEventQuery
                .ApplyPagination(request)
                .ToListAsync(cancellationToken);

            var result = sectionEventsList.Select(sectionEventMapper.MapToListViewModel);
            return result.AsPagedResult(request, await sectionEventQuery.CountAsync(cancellationToken));
        }
    }
}

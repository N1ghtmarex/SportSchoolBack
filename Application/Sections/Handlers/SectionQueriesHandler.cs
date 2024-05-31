using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Sections.Dtos;
using Application.Sections.Queries;
using Core.EntityFramework.Features.SearchPagination;
using Core.EntityFramework.Features.SearchPagination.Models;
using Core.Exceptions;
using Domain;
using MediatR;

using Microsoft.EntityFrameworkCore;


namespace Application.Sections.Handlers
{
    internal class SectionQueriesHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, ISectionMapper sectionMapper, IClientService clientService,
        ICoachService coachService) :
        IRequestHandler<GetSectionsListQuery, PagedResult<SectionListViewModel>>, IRequestHandler<GetSectionQuery, SectionListViewModel>, 
        IRequestHandler<GetUserSectionsQuery, PagedResult<SectionListViewModel>>, IRequestHandler<IsClientInSectionQuery, bool>
    {
        public string directory = @"C:\Users\andre\source\repos\SportSchool\images\sections";
        private readonly string imagesDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.ToString() ?? string.Empty, "images", "sections");

        public async Task<PagedResult<SectionListViewModel>> Handle(GetSectionsListQuery request, CancellationToken cancellationToken)
        {
            var sectionQuery = dbContext.Sections
                .AsNoTracking()
                .Include(x => x.Sport)
                .Include(x => x.Room)
                .Include(x => x.Coach)
                .OrderBy(x => x.Name)
                .ApplySearch(request, x => x.Name);

            if (request.SportId != null)
            {
                sectionQuery = sectionQuery
                    .Where(x => x.SportId == request.SportId);
            }

            var sectionsList = await sectionQuery
                .ApplyPagination(request)
            .ToListAsync(cancellationToken);

            var result = sectionsList.Select(sectionMapper.MapToListViewModel);
            return result.AsPagedResult(request, await sectionQuery.CountAsync(cancellationToken));
        }

        public async Task<SectionListViewModel> Handle(GetSectionQuery request, CancellationToken cancellationToken)
        {
            var existingSection = await dbContext.Sections
                .AsNoTracking()
                .Where(x => x.Id == request.SectionId)
                .Include(x => x.Sport)
                .Include(x => x.Room)
                .Include(x => x.Coach)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new ObjectNotFoundException($"Секция с идентификатором {request.SectionId} не найдена!");


            return sectionMapper.MapToListViewModel(existingSection);
        }

        public async Task<PagedResult<SectionListViewModel>> Handle(GetUserSectionsQuery request, CancellationToken cancellationToken)
        {


            var sectionQuery = dbContext.Sections
                .AsNoTracking()
                .Include(x => x.Sport)
                .Include(x => x.Room)
                .Include(x => x.Coach)
                .AsQueryable();

            if (contextAccessor.UserRoles.Contains("Coach"))
            {
                var coach = await coachService.GetCoachAync(contextAccessor.IdentityUserId, false, cancellationToken);

                sectionQuery = sectionQuery.Where(x => x.CoachId == coach.Id);
            }
            else
            {
                var client = await clientService.GetClientAsync(contextAccessor.IdentityUserId, true, cancellationToken);

                sectionQuery = sectionQuery.Where(x => x.Client.Contains(client));
            }

            if (request.SportId != null)
            {
                sectionQuery = sectionQuery.Where(x => x.SportId == request.SportId);
            }

            sectionQuery = sectionQuery
                .OrderBy(x => x.Name)
                .ApplySearch(request, x => x.Name);

            var sectionsList = await sectionQuery
                .ApplyPagination(request)
                .ToListAsync(cancellationToken);

            var result = sectionsList.Select(sectionMapper.MapToListViewModel);
            return result.AsPagedResult(request, await sectionQuery.CountAsync(cancellationToken));
        }

        public async Task<bool> Handle(IsClientInSectionQuery request, CancellationToken cancellationToken)
        {
            var sectionQuery = dbContext.Sections
                .AsNoTracking()
                .Where(x => x.Id == request.SectionId);

            if (contextAccessor.UserRoles.Contains("Coach"))
            {
                var coach = await coachService.GetCoachAync(contextAccessor.IdentityUserId, false, cancellationToken);
                sectionQuery = sectionQuery.Where(x => x.CoachId == coach.Id);
            }
            else
            {
                var client = await clientService.GetClientAsync(contextAccessor.IdentityUserId, true, cancellationToken);
                sectionQuery = sectionQuery.Where(x => x.Client.Contains(client));
            }
            

            var section = await sectionQuery
                .SingleOrDefaultAsync(cancellationToken);

            if (section == null)
            {
                return false;
            }    
            else
            {
                return true;
            }
        }
    }
}

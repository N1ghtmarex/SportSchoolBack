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
    internal class SectionQueriesHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, ISectionMapper sectionMapper, IClientService clientService) :
        IRequestHandler<GetSectionsListQuery, PagedResult<SectionListViewModel>>, IRequestHandler<GetSectionQuery, SectionListViewModel>, 
        IRequestHandler<GetUserSectionsQuery, PagedResult<SectionListViewModel>>, IRequestHandler<IsClientInSectionQuery, bool>
    {
        public string directory = @"C:\Users\andre\source\repos\SportSchool\images\sections";
        private readonly string imagesDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.ToString() ?? string.Empty, "images", "sections");

        public async Task<PagedResult<SectionListViewModel>> Handle(GetSectionsListQuery request, CancellationToken cancellationToken)
        {
            var sectionQuery = dbContext.Sections
                .AsNoTracking();

            if (request.SportId != null)
            {
                sectionQuery = sectionQuery
                    .Where(x => x.SportId == request.SportId);
            }

            sectionQuery = sectionQuery
                .Include(x => x.Sport)
                .Include(x => x.Room)
                .Include(x => x.Coach)
                .OrderBy(x => x.Name)
                .ApplySearch(request, x => x.Name);

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
            var client = await clientService.GetClientAsync(contextAccessor.IdentityUserId, true, cancellationToken);

            var sectionQuery = dbContext.Sections
                .AsNoTracking()
                .Include(x => x.Sport)
                .Include(x => x.Room)
                .Include(x => x.Coach)
                .Where(x => x.Client.Contains(client))
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
            var client = await clientService.GetClientAsync(contextAccessor.IdentityUserId, true, cancellationToken);

            var section = await dbContext.Sections
                .AsNoTracking()
                .Where(x => x.Id == request.SectionId)
                .Where(x => x.Client.Contains(client))
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

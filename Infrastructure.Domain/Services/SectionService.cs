using Abstractions.Services;
using Ardalis.GuardClauses;
using Core.Exceptions;
using Core.Utils;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.Services
{
    public class SectionService(ApplicationDbContext dbContext) : ISectionService
    {
        public async Task<Section> GetSectionAsync(Guid id, bool includeSport, bool includeRoom, bool includeCoach, CancellationToken cancellationToken)
        {
            Defend.Against.Null(id, nameof(id));

            var sectionQuery = dbContext.Sections.Where(x => x.Id == id);

            if (includeSport)
            {
                sectionQuery = sectionQuery.Include(x => x.Sport);
            }

            if (includeRoom)
            {
                sectionQuery = sectionQuery.Include(x => x.Room);
            }

            if (includeCoach)
            {
                sectionQuery = sectionQuery.Include(x => x.Coach);
            }

            var section = await sectionQuery.SingleOrDefaultAsync(cancellationToken);

            if (section == null)
            {
                throw new ObjectNotFoundException(
                    $"Секция с идентификатором \"${id}\" не найдена!");
            }

            return section;
        }
    }
}

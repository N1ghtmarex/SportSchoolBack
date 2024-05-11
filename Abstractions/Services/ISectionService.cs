using Domain.Entities;

namespace Abstractions.Services
{
    public interface ISectionService
    {
        public Task<Section> GetSectionAsync(Guid id, bool includeSport, bool includeRoom, bool includeCoach, CancellationToken cancellationToken);
    }
}

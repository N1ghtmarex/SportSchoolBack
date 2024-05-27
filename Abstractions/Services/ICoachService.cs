using Domain.Entities;

namespace Abstractions.Services
{
    public interface ICoachService
    {
        Task<Coach> GetCoachAync(string id, bool includeSection, CancellationToken cancellationToken);
    }
}

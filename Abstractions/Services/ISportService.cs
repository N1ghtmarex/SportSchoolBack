using Domain.Entities;

namespace Abstractions.Services
{
    public interface ISportService
    {
        Task<Sport> GetSportAsync(Guid id, CancellationToken cancellationToken);
    }
}

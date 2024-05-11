using Domain.Entities;

namespace Abstractions.Services
{
    public interface IClientService
    {
        Task<Client> GetClientAsync(string id, bool includeSection, CancellationToken cancellationToken);
    }
}

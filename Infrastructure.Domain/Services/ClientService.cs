using Abstractions.Services;
using Ardalis.GuardClauses;
using Core.Exceptions;
using Core.Utils;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.Services
{
    public class ClientService(ApplicationDbContext dbContext) : IClientService
    {
        public async Task<Client> GetClientAsync(string id, bool includeSection, CancellationToken cancellationToken)
        {
            Defend.Against.Null(id, nameof(id));

            var clientId = Guid.Parse(id);

            var clientQuery = dbContext.Clients.Where(x => x.ExternalId == clientId);

            if (includeSection)
            {
                clientQuery = clientQuery.Include(x => x.Section);
            }

            var client = await clientQuery.SingleOrDefaultAsync(cancellationToken);

            if (client == null)
            {
                throw new ObjectNotFoundException(
                    $"Пользователь с внешним идентификатором \"{id}\" не найден!");
            }

            return client;
        }
    }
}

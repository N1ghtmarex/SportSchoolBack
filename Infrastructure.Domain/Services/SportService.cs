using Abstractions.Services;
using Ardalis.GuardClauses;
using Core.Exceptions;
using Core.Utils;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.Services
{
    internal class SportService(ApplicationDbContext dbContext) : ISportService
    {
        public async Task<Sport> GetSportAsync(Guid id, CancellationToken cancellationToken)
        {
            Defend.Against.Null(id, nameof(id));

            var sport = await dbContext.Sports
                .AsNoTracking()
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            if (sport == null)
            {
                throw new ObjectNotFoundException(
                    $"Спорт с идентификатором \"${id}\" не найден!");
            }

            return sport;
        }
    }
}

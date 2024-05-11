using Abstractions.Services;
using Ardalis.GuardClauses;
using Core.Exceptions;
using Core.Utils;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.Services
{
    public class CoachService(ApplicationDbContext dbContext) : ICoachService
    {
        public async Task<Coach> GetCoachAync(string id, CancellationToken cancellationToken)
        {
            Defend.Against.Null(id, nameof(id));

            var coachId = Guid.Parse(id);

            var coachQuery = dbContext.Coachs
                .AsNoTracking()
                .Where(x => x.ExternalId == coachId);

            var coach = await coachQuery.SingleOrDefaultAsync(cancellationToken);

            if (coach == null)
            {
                throw new ObjectNotFoundException(
                    $"Тренер с внешним идентификатором \"${id}\" не найден!");
            }

            return coach;
        }
    }
}

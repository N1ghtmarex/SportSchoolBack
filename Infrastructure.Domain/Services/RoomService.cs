using Abstractions.Services;
using Ardalis.GuardClauses;
using Core.Exceptions;
using Core.Utils;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.Services
{
    public class RoomService(ApplicationDbContext dbContext) : IRoomService
    {
        public async Task<Room> GetRoomAsync(Guid id, CancellationToken cancellationToken)
        {
            Defend.Against.Null(id, nameof(id));

            var room = await dbContext.Rooms
                .AsNoTracking()
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            if (room == null)
            {
                throw new ObjectNotFoundException(
                    $"Зал с идентификатором \"{id}\" не найден!");
            }

            return room;
        }
    }
}

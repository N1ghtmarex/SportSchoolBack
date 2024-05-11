using Domain.Entities;

namespace Abstractions.Services
{
    public interface IRoomService
    {
        Task<Room> GetRoomAsync(Guid id, CancellationToken cancellationToken);
    }
}

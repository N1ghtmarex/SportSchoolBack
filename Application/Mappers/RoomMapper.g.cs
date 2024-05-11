using Application.Rooms;
using Application.Rooms.Dtos;
using Domain.Entities;

namespace Application.Rooms
{
    public partial class RoomMapper : IRoomMapper
    {
        public Room MapToEntity(CreateRoomModel p1)
        {
            return p1 == null ? null : new Room() {Name = p1.Name};
        }
        public RoomListViewModel MapToListViewModel(Room p2)
        {
            return p2 == null ? null : new RoomListViewModel()
            {
                Id = p2.Id,
                Name = p2.Name
            };
        }
        public RoomViewModel MapToViewModel(Room p3)
        {
            return p3 == null ? null : new RoomViewModel()
            {
                Id = p3.Id,
                Name = p3.Name
            };
        }
    }
}
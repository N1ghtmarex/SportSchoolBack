using Application.Rooms.Dtos;
using Domain.Entities;
using Mapster;

namespace Application.Rooms
{
    [Mapper]
    public interface IRoomMapper
    {
        Room MapToEntity(CreateRoomModel model);
        RoomListViewModel MapToListViewModel(Room room);
        RoomViewModel MapToViewModel(Room room);
    }

    internal class RoomMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        { 
            config.NewConfig<CreateRoomModel, Room>()
                .Map(d => d.Name, src => src.Name);

            config.NewConfig<Room, RoomListViewModel>()
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Id, src => src.Id);

            config.NewConfig<Room, RoomViewModel>()
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Id, src => src.Id);
        }
    }
}

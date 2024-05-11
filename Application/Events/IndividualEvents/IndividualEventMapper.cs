using Application.Coachs.Dtos;
using Application.Events.IndividualEvents.Dtos;
using Application.Rooms.Dtos;
using Application.Sports.Dtos;
using Domain.Entities;
using Mapster;

namespace Application.Events.IndividualEvents
{
    [Mapper]
    public interface IIndividualEventMapper
    {
        IndividualEventListViewModel MapToListViewModel(IndividualEvent individualEvent);
    }

    internal class IndividualEventMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<IndividualEvent, IndividualEventListViewModel>()
                .Map(d => d.Id, src => src.Id)
                .Map(d => d.StartTime, src => src.StartDate)
                .Map(d => d.EndTime, src => src.EndDate)
                .Map(d => d.Coach, src => src.Coach)
                .Map(d => d.Sport, src => src.Sport)
                .Map(d => d.Room, src => src.Room)
                .Map(d => d.ClientId, src => src.ClientId);

            config.NewConfig<Coach, CoachListViewModel>()
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Surname, src => src.Surname);

            config.NewConfig<Sport, SportListViewModel>()
                .Map(d => d.Name, src => src.Name);

            config.NewConfig<Room, RoomListViewModel>()
                .Map(d => d.Name, src => src.Name);
        }
    }
}

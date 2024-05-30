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
                .Map(d => d.Id, src => src.Id)
                .Map(d => d.ExternalId, src => src.ExternalId)
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Surname, src => src.Surname)
                .Map(d => d.Phone, src => src.Phone)
                .Map(d => d.Institution, src => src.Institution)
                .Map(d => d.Faculty, src => src.Faculty)
                .Map(d => d.Speciality, src => src.Speciality)
                .Map(d => d.EducationForm, src => src.EducationForm)
                .Map(d => d.Qualification, src => src.Qualification)
                .Map(d => d.Job, src => src.Job)
                .Map(d => d.JobTitle, src => src.JobTitle)
                .Map(d => d.JobPeriod, src => src.JobPeriod);

            config.NewConfig<Sport, SportListViewModel>()
                .Map(d => d.Name, src => src.Name);

            config.NewConfig<Room, RoomListViewModel>()
                .Map(d => d.Name, src => src.Name);
        }
    }
}

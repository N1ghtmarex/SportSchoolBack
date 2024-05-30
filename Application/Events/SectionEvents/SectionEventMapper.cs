using Application.Coachs.Dtos;
using Application.Events.SectionEvents.Dtos;
using Application.Rooms.Dtos;
using Application.Sections.Dtos;
using Application.Sports.Dtos;
using Domain.Entities;
using Mapster;

namespace Application.Events.SectionEvents
{
    [Mapper]
    public interface ISectionEventMapper
    {
        SectionEventListViewModel MapToListViewModel(SectionEvent sectionEvent);
    }

    internal class SectionEventMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SectionEvent, SectionEventListViewModel>()
                .Map(d => d.DayOfWeek, src => src.DayOfWeek)
                .Map(d => d.StartTime, src => src.StartTime)
                .Map(d => d.EndTime, src => src.EndTime)
                .Map(d => d.Period, src => src.Period)
                .Map(d => d.Section, src => src.Section);

            config.NewConfig<Section, SectionListViewModel>()
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Description, src => src.Description)
                .Map(d => d.Coach, src => src.Coach)
                .Map(d => d.Room, src => src.Room)
                .Map(d => d.Sport, src => src.Sport);

            config.NewConfig<Room, RoomViewModel>()
                .Map(d => d.Name, src => src.Name);

            config.NewConfig<Sport, SportViewModel>()
                .Map(d => d.Name, src => src.Name);

            config.NewConfig<Coach, CoachViewModel>()
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
        }
    }
}

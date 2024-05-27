using Application.Coachs.Dtos;
using Application.Events.SectionEvents;
using Application.Events.SectionEvents.Dtos;
using Application.Rooms.Dtos;
using Application.Sections.Dtos;
using Application.Sports.Dtos;
using Domain.Entities;

namespace Application.Events.SectionEvents
{
    public partial class SectionEventMapper : ISectionEventMapper
    {
        public SectionEventListViewModel MapToListViewModel(SectionEvent p1)
        {
            return p1 == null ? null : new SectionEventListViewModel()
            {
                Id = p1.Id,
                DayOfWeek = p1.DayOfWeek,
                StartTime = p1.StartTime,
                EndTime = p1.EndTime,
                Period = p1.Period,
                Section = p1.Section == null ? null : new SectionListViewModel()
                {
                    Id = p1.Section.Id,
                    Name = p1.Section.Name,
                    Description = p1.Section.Description,
                    Room = p1.Section.Room == null ? null : new RoomListViewModel()
                    {
                        Id = p1.Section.Room.Id,
                        Name = p1.Section.Room.Name
                    },
                    Sport = p1.Section.Sport == null ? null : new SportListViewModel()
                    {
                        Id = p1.Section.Sport.Id,
                        Name = p1.Section.Sport.Name
                    },
                    Coach = p1.Section.Coach == null ? null : new CoachListViewModel()
                    {
                        Id = p1.Section.Coach.Id,
                        ExternalId = p1.Section.Coach.ExternalId,
                        Name = p1.Section.Coach.Name,
                        Surname = p1.Section.Coach.Surname
                    }
                }
            };
        }
    }
}
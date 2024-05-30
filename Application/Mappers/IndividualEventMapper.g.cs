using Application.Coachs.Dtos;
using Application.Events.IndividualEvents;
using Application.Events.IndividualEvents.Dtos;
using Application.Rooms.Dtos;
using Application.Sports.Dtos;
using Domain.Entities;

namespace Application.Events.IndividualEvents
{
    public partial class IndividualEventMapper : IIndividualEventMapper
    {
        public IndividualEventListViewModel MapToListViewModel(IndividualEvent p1)
        {
            return p1 == null ? null : new IndividualEventListViewModel()
            {
                Id = p1.Id,
                StartTime = p1.StartDate,
                EndTime = p1.EndDate,
                Coach = p1.Coach == null ? null : new CoachListViewModel()
                {
                    Id = p1.Coach.Id,
                    ExternalId = p1.Coach.ExternalId,
                    Name = p1.Coach.Name,
                    Surname = p1.Coach.Surname,
                    Phone = p1.Coach.Phone,
                    Institution = p1.Coach.Institution,
                    Faculty = p1.Coach.Faculty,
                    Speciality = p1.Coach.Speciality,
                    EducationForm = p1.Coach.EducationForm,
                    Qualification = p1.Coach.Qualification,
                    Job = p1.Coach.Job,
                    JobTitle = p1.Coach.JobTitle,
                    JobPeriod = p1.Coach.JobPeriod
                },
                Sport = p1.Sport == null ? null : new SportListViewModel()
                {
                    Id = p1.Sport.Id,
                    Name = p1.Sport.Name
                },
                Room = p1.Room == null ? null : new RoomListViewModel()
                {
                    Id = p1.Room.Id,
                    Name = p1.Room.Name
                },
                ClientId = p1.ClientId
            };
        }
    }
}
using System;
using Application.Coachs.Dtos;
using Application.Rooms.Dtos;
using Application.Sections;
using Application.Sections.Dtos;
using Application.Sports.Dtos;
using Domain.Entities;

namespace Application.Sections
{
    public partial class SectionMapper : ISectionMapper
    {
        public Section MapToEntity(ValueTuple<CreateSectionModel, Guid> p1)
        {
            return new Section()
            {
                Name = p1.Item1.Name,
                Description = p1.Item1.Description,
                SportId = p1.Item1.SportId,
                CoachId = p1.Item2,
                RoomId = p1.Item1.RoomId
            };
        }
        public SectionListViewModel MapToListViewModel(Section p2)
        {
            return p2 == null ? null : new SectionListViewModel()
            {
                Id = p2.Id,
                Name = p2.Name,
                Description = p2.Description,
                Room = p2.Room == null ? null : new RoomListViewModel()
                {
                    Id = p2.Room.Id,
                    Name = p2.Room.Name
                },
                Sport = p2.Sport == null ? null : new SportListViewModel()
                {
                    Id = p2.Sport.Id,
                    Name = p2.Sport.Name
                },
                Coach = p2.Coach == null ? null : new CoachListViewModel()
                {
                    Id = p2.Coach.Id,
                    ExternalId = p2.Coach.ExternalId,
                    Name = p2.Coach.Name,
                    Surname = p2.Coach.Surname,
                    Phone = p2.Coach.Phone,
                    Institution = p2.Coach.Institution,
                    Faculty = p2.Coach.Faculty,
                    Speciality = p2.Coach.Speciality,
                    EducationForm = p2.Coach.EducationForm,
                    Qualification = p2.Coach.Qualification,
                    Job = p2.Coach.Job,
                    JobTitle = p2.Coach.JobTitle,
                    JobPeriod = p2.Coach.JobPeriod
                }
            };
        }
    }
}
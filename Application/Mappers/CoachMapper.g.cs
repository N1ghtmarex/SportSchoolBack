using System;
using Application.Coachs;
using Application.Coachs.Dtos;
using Domain.Entities;

namespace Application.Coachs
{
    public partial class CoachMapper : ICoachMapper
    {
        public Coach MapToEntity(ValueTuple<CreateCoachModel, string, string, string> p1)
        {
            return new Coach()
            {
                ExternalId = p1.Item1.ExternalId,
                Name = p1.Item2,
                Surname = p1.Item3,
                Institution = p1.Item1.Institution,
                Faculty = p1.Item1.Faculty,
                Speciality = p1.Item1.Speciality,
                EducationForm = p1.Item1.EducationForm,
                Qualification = p1.Item1.Qualification,
                Job = p1.Item1.Job,
                JobTitle = p1.Item1.JobTitle,
                JobPeriod = p1.Item1.JobPeriod
            };
        }
        public CoachListViewModel MapToListViewModel(Coach p2)
        {
            return p2 == null ? null : new CoachListViewModel()
            {
                Id = p2.Id,
                ExternalId = p2.ExternalId,
                Name = p2.Name,
                Surname = p2.Surname
            };
        }
        public CoachViewModel MapToViewModel(Coach p3)
        {
            return p3 == null ? null : new CoachViewModel()
            {
                Id = p3.Id,
                ExternalId = p3.ExternalId,
                Name = p3.Name,
                Surname = p3.Surname
            };
        }
    }
}
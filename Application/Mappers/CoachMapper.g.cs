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
                Phone = p1.Item4,
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
                Surname = p2.Surname,
                Phone = p2.Phone,
                Institution = p2.Institution,
                Faculty = p2.Faculty,
                Speciality = p2.Speciality,
                EducationForm = p2.EducationForm,
                Qualification = p2.Qualification,
                Job = p2.Job,
                JobTitle = p2.JobTitle,
                JobPeriod = p2.JobPeriod
            };
        }
        public CoachViewModel MapToViewModel(Coach p3)
        {
            return p3 == null ? null : new CoachViewModel()
            {
                Id = p3.Id,
                ExternalId = p3.ExternalId,
                Name = p3.Name,
                Surname = p3.Surname,
                Phone = p3.Phone,
                Institution = p3.Institution,
                Faculty = p3.Faculty,
                Speciality = p3.Speciality,
                EducationForm = p3.EducationForm,
                Qualification = p3.Qualification,
                Job = p3.Job,
                JobTitle = p3.JobTitle,
                JobPeriod = p3.JobPeriod
            };
        }
    }
}
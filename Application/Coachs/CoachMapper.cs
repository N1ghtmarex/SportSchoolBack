using Application.Coachs.Dtos;
using Domain.Entities;
using Mapster;

namespace Application.Coachs
{
    [Mapper]
    public interface ICoachMapper
    {
        Coach MapToEntity((CreateCoachModel model, string name, string surname, string phone) src);
        CoachListViewModel MapToListViewModel(Coach coach);
        CoachViewModel MapToViewModel(Coach coach);
    }

    internal class CoachMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(CreateCoachModel Model, string Name, string Surname, string Phone), Coach>()
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Surname, src => src.Surname)
                .Map(d => d.Phone, src => src.Phone)
                .Map(d => d.ExternalId, src => src.Model.ExternalId)
                .Map(d => d.Institution, src => src.Model.Institution)
                .Map(d => d.Faculty, src => src.Model.Faculty)
                .Map(d => d.Speciality, src => src.Model.Speciality)
                .Map(d => d.EducationForm, src => src.Model.EducationForm)
                .Map(d => d.Qualification, src => src.Model.Qualification)
                .Map(d => d.Job, src => src.Model.Job)
                .Map(d => d.JobTitle, src => src.Model.JobTitle)
                .Map(d => d.JobPeriod, src => src.Model.JobPeriod);


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
        }
    }
}

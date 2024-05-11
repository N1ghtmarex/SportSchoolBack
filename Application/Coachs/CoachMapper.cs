using Application.Coachs.Dtos;
using Domain.Entities;
using Mapster;

namespace Application.Coachs
{
    [Mapper]
    public interface ICoachMapper
    {
        Coach MapToEntity((string name, string surname, Guid externalId) src);
        CoachListViewModel MapToListViewModel(Coach coach);
        CoachViewModel MapToViewModel(Coach coach);
    }

    internal class CoachMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(string Name, string Surname, Guid ExternalId), Coach>()
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Surname, src => src.Surname)
                .Map(d => d.ExternalId, src => src.ExternalId);

            config.NewConfig<Coach, CoachListViewModel>()
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Surname, src => src.Surname)
                .Map(d => d.Id, src => src.Id)
                .Map(d => d.ExternalId, src => src.ExternalId);
        }
    }
}

using Application.Sports.Dtos;
using Domain.Entities;
using Mapster;

namespace Application.Sports
{
    [Mapper]
    public interface ISportMapper
    {
        Sport MapToEntity(CreateSportModel model);
        SportListViewModel MapToListViewModel(Sport sport);
        SportViewModel MapToViewModel(Sport sport);
    }

    internal class SportMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateSportModel, Sport>()
                .Map(d => d.Name, s => s.Name);

            config.NewConfig<Sport, SportListViewModel>()
            .Map(d => d.Name, src => src.Name)
            .Map(d => d.Id, src => src.Id);

            config.NewConfig<Sport, SportViewModel>()
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Id, src => src.Id);

        }
    }
}

using Application.Coachs.Dtos;
using Application.Rooms.Dtos;
using Application.Sections.Dtos;
using Application.Sports.Dtos;
using Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace Application.Sections
{
    [Mapper]
    public interface ISectionMapper
    {
        Section MapToEntity((CreateSectionModel model, Guid coachId) src);
        SectionListViewModel MapToListViewModel(Section section);
        //SectionViewModel MapToViewModel(Section section);
    }

    internal class SectionMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(CreateSectionModel Model, Guid CoachId), Section>()
                .Map(d => d.Name, src => src.Model.Name)
                .Map(d => d.SportId, src => src.Model.SportId)
                .Map(d => d.CoachId, src => src.CoachId)
                .Map(d => d.RoomId, src => src.Model.RoomId);

            config.NewConfig<Section, SectionListViewModel>()
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Id, src => src.Id)
                .Map(d => d.Room, src => src.Room)
                .Map(d => d.Sport, src => src.Sport);
               
            config.NewConfig<Room, RoomViewModel>()
                .Map(d => d.Name, src => src.Name);

            config.NewConfig<Sport, SportViewModel>()
                .Map(d => d.Name, src => src.Name);

            config.NewConfig<Coach, CoachViewModel>()
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Surname, src => src.Surname);
        }
    }
}

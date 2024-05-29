using Domain.Entities;
using Mapster;

namespace Application.Clients
{
    [Mapper]
    public interface IClientMapper
    {
        Client MapToEntity((string name, string surname, string Phone, Guid externalId) src);
    }

    internal class ClientMapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config) 
        {
            config.NewConfig<(string Name, string Surname, string Phone, Guid ExternalId), Client>()
                .Map(d => d.Name, src => src.Name)
                .Map(d => d.Surname, src => src.Surname)
                .Map(d => d.Phone, src => src.Phone)
                .Map(d => d.ExternalId, src => src.ExternalId);
        }
    }
}

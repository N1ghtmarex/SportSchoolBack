using System;
using Application.Clients;
using Application.Clients.Dtos;
using Domain.Entities;

namespace Application.Clients
{
    public partial class ClientMapper : IClientMapper
    {
        public Client MapToEntity(ValueTuple<string, string, string, Guid> p1)
        {
            return new Client()
            {
                ExternalId = p1.Item4,
                Name = p1.Item1,
                Surname = p1.Item2,
                Phone = p1.Item3
            };
        }
        public ClientListViewModel MapToListViewModel(Client p2)
        {
            return p2 == null ? null : new ClientListViewModel()
            {
                Id = p2.Id,
                ExternalId = p2.ExternalId,
                Name = p2.Name,
                Surname = p2.Surname,
                Phone = p2.Phone
            };
        }
        public ClientViewModel MapToViewModel(Client p3)
        {
            return p3 == null ? null : new ClientViewModel()
            {
                Id = p3.Id,
                ExternalId = p3.ExternalId,
                Name = p3.Name,
                Surname = p3.Surname,
                Phone = p3.Phone
            };
        }
    }
}
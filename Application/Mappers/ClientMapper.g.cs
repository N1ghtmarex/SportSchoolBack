using System;
using Application.Clients;
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
    }
}
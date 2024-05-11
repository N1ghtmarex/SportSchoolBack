using System;
using Application.Clients;
using Domain.Entities;

namespace Application.Clients
{
    public partial class ClientMapper : IClientMapper
    {
        public Client MapToEntity(ValueTuple<string, string, Guid> p1)
        {
            return new Client()
            {
                ExternalId = p1.Item3,
                Name = p1.Item1,
                Surname = p1.Item2
            };
        }
    }
}
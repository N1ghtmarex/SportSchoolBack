using System;
using Application.Coachs;
using Application.Coachs.Dtos;
using Domain.Entities;

namespace Application.Coachs
{
    public partial class CoachMapper : ICoachMapper
    {
        public Coach MapToEntity(ValueTuple<string, string, Guid> p1)
        {
            return new Coach()
            {
                ExternalId = p1.Item3,
                Name = p1.Item1,
                Surname = p1.Item2
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
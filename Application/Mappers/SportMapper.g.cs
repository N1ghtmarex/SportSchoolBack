using Application.Sports;
using Application.Sports.Dtos;
using Domain.Entities;

namespace Application.Sports
{
    public partial class SportMapper : ISportMapper
    {
        public Sport MapToEntity(CreateSportModel p1)
        {
            return p1 == null ? null : new Sport() {Name = p1.Name};
        }
        public SportListViewModel MapToListViewModel(Sport p2)
        {
            return p2 == null ? null : new SportListViewModel()
            {
                Id = p2.Id,
                Name = p2.Name
            };
        }
        public SportViewModel MapToViewModel(Sport p3)
        {
            return p3 == null ? null : new SportViewModel()
            {
                Id = p3.Id,
                Name = p3.Name
            };
        }
    }
}
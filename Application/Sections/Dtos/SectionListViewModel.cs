using Application.Coachs.Dtos;
using Application.Rooms.Dtos;
using Application.Sports.Dtos;

namespace Application.Sections.Dtos
{
    public class SectionListViewModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public RoomListViewModel Room { get; set; }
        public SportListViewModel Sport { get; set; }
        public CoachListViewModel Coach { get; set; }
    }
}

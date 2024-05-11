using Application.Coachs.Dtos;
using Application.Rooms.Dtos;
using Application.Sports.Dtos;

namespace Application.Events.IndividualEvents.Dtos
{
    public class IndividualEventListViewModel
    {
        public required Guid Id { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public required CoachListViewModel Coach {  get; set; }
        public required SportListViewModel Sport { get; set; }
        public required RoomListViewModel Room { get; set; }
        public Guid? ClientId { get; set; }
    }
}

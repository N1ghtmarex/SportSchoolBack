namespace Domain.Entities
{
    public class IndividualEvent: BaseEntity<Guid>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CoachId { get; set; }
        public Coach Coach { get; set; }
        public Guid SportId { get; set; }
        public Sport Sport { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public Guid? ClientId { get; set; }
        public Client? Client { get; set; }
    }
}

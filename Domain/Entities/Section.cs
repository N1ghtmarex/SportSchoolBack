namespace Domain.Entities
{
    public class Section: BaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid SportId { get; set; }
        public Sport Sport { get; set; } = null!;

        public Guid CoachId { get; set; }
        public Coach Coach { get; set; } = null!;

        public List<Client>? Client { get; set; } = new List<Client>();

        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;
    }
}

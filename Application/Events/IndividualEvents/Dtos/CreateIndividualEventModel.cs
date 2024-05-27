namespace Application.Events.IndividualEvents.Dtos
{
    public class CreateIndividualEventModel
    {
        public string StartDate { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public Guid SportId { get; set; }
        public Guid RoomId { get; set; }
    }
}

namespace Application.Events.SectionEvents.Dtos
{
    public class CreateSectionEventModel
    {
        public required int DayOfWeek { get; set; }
        public required string StartTime { get; set; }
        public required string EndTime { get; set; }

        public required string Period { get; set; }

        public required Guid SectionId { get; set; }
    }
}

namespace Domain.Entities
{
    public class SectionEvent: BaseEntity<Guid>
    {
        public int DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateOnly Period { get; set; }
        public Guid SectionId { get; set; }
        public Section Section { get; set; }
    }
}

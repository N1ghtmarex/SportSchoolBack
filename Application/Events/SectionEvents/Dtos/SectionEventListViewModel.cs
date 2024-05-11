using Application.Sections.Dtos;

namespace Application.Events.SectionEvents.Dtos
{
    public class SectionEventListViewModel
    {
        public required Guid Id { get; set; }
        public int DayOfWeek { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
        public required DateOnly Period { get; set; }
        public required SectionListViewModel Section {  get; set; }
    }
}

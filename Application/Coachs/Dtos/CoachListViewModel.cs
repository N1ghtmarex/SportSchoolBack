namespace Application.Coachs.Dtos
{
    public class CoachListViewModel
    {
        public required Guid Id { get; set; }
        public string? ImageFileName { get; set; }
        public required Guid ExternalId { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Phone { get; set; }
        public required string Institution { get; set; }
        public required string Faculty { get; set; }
        public required string Speciality { get; set; }
        public required string EducationForm { get; set; }
        public required string Qualification { get; set; }
        public required string Job { get; set; }
        public required string JobTitle { get; set; }
        public required string JobPeriod { get; set; }
    }
}

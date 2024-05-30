namespace Application.Coachs.Dtos
{
    public class CreateCoachModel
    {
        public required Guid ExternalId { get; set; }
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

namespace Domain.Entities
{
    public class Coach : BaseEntity<Guid>
    {
        public Guid ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Phone {  get; set; } = string.Empty;
        public string Institution {  get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Speciality { get; set; } = string.Empty;
        public string EducationForm { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string Job { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string JobPeriod { get; set; } = string.Empty;

        public List<Section>? Section { get; set; } = new List<Section>();
    }
}

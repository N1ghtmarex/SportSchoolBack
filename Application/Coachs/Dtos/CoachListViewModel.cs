namespace Application.Coachs.Dtos
{
    public class CoachListViewModel
    {
        public required Guid Id { get; set; }
        public required Guid ExternalId { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
    }
}

namespace Application.Clients.Dtos
{
    public class ClientListViewModel
    {
        public required Guid Id { get; set; }
        public required Guid ExternalId { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Phone { get; set; }
    }
}

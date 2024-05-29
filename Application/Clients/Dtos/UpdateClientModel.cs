using Microsoft.AspNetCore.Http;

namespace Application.Clients.Dtos
{
    public class UpdateClientModel
    {
        public IFormFile? Image { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Phone { get; set; }
    }
}

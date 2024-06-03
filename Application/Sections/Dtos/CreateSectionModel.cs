using Microsoft.AspNetCore.Http;

namespace Application.Sections.Dtos
{
    public class CreateSectionModel
    {
        public IFormFile? Image { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required Guid SportId { get; set; }
        public required Guid RoomId { get; set; }
    }
}

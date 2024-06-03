using Microsoft.AspNetCore.Http;

namespace Application.Register.Dtos
{
    public class RegisterUserModel
    {
        public IFormFile? Image { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Phone { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
    }
}

using Microsoft.AspNetCore.Http;

namespace Abstractions.Services
{
    public interface IImageService
    {
        string SaveUserImage(IFormFile file, string? oldFileName);
    }
}

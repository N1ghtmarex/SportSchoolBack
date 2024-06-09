using Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Domain.Services
{
    public class ImageService : IImageService
    {
        public string SaveUserImage(IFormFile file, string? oldFileName)
        {
            //var imagesDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.ToString() ?? string.Empty, "SportSchool", "wwwroot", "users");
            var imagesDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.ToString() ?? string.Empty, "app", "wwwroot", "users");
            var fileName = $"{Guid.NewGuid()}.jpeg";
            var filePath = Path.Combine(imagesDirectory, fileName);

            if (oldFileName != null)
            {
                var olfFilePath = Path.Combine(imagesDirectory, oldFileName);
                File.Delete(olfFilePath);
            }

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return fileName;
        }
    }
}

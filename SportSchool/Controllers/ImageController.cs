using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSchool.StartupConfigurations;

namespace SportSchool.Controllers
{
    [Route("api/admin/images")]
    [Authorize(Policy = KeycloakAuthConfiguration.AdminApiPolicy)]
    [ApiExplorerSettings(GroupName = "sportschool")]
    public class ImageController(ISender sender) : BaseController
    {
        private readonly string imagesDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.ToString() ?? string.Empty, "images", "sections");

        [HttpGet("{ImageId}")]
        public async Task<IActionResult> GetImage(string ImageId) 
        {
            string path = Path.Combine(imagesDirectory, $"{ImageId}.jpeg");

            if (!System.IO.File.Exists(path))
            {
                return PhysicalFile(Path.Combine(imagesDirectory, "default.jpeg"), "image/jpeg");
            }

            return PhysicalFile(path, "image/jpeg");
        }
    }
}

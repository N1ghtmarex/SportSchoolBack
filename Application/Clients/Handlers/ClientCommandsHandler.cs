using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Clients;
using Application.Clients.Commands;
using Domain;
using MediatR;

namespace Application.Users.Handlers
{
    internal class ClientCommandsHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, IClientMapper clientMapper, IClientService clientService) :
        IRequestHandler<UpdateClientCommand, string>
    {
        public async Task<string> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientAsync(contextAccessor.IdentityUserId, false, cancellationToken);

            if (request.Body.Image != null)
            {
                var imagesDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.ToString() ?? string.Empty, "SportSchool", "wwwroot", "users");
                var filePath = Path.Combine(imagesDirectory, $"{client.ExternalId}.jpeg");

                File.Delete(filePath);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    request.Body.Image.CopyTo(fileStream);
                }
            }

            client.Name = request.Body.Name;
            client.Surname = request.Body.Surname;
            client.Phone = request.Body.Phone;

            await dbContext.SaveChangesAsync(cancellationToken);

            return "Данные обновлены!";
        }
    }
}

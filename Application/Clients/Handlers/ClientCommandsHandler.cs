using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Clients;
using Application.Clients.Commands;
using Application.Register.Handlers;
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

            var httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/realms/master/protocol/openid-connect/token");

            var collection = new List<KeyValuePair<string, string>>
            {
                new("username", "admin"),
                new("grant_type", "password"),
                new("client_id", "admin-cli"),
                new("password", "admin")
            };

            var content = new FormUrlEncodedContent(collection);
            httpRequest.Content = content;

            var response = await httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var token = System.Text.Json.JsonSerializer.Deserialize<Token>(responseString);

            
            httpRequest = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:8080/admin/realms/SportSchool/users/{client.ExternalId}");
            httpRequest.Headers.Add("Authorization", $"Bearer {token!.access_token}");

            var jsonString = new StringContent($@"
            {{
                ""firstName"": ""{request.Body.Name}"",
                ""lastName"": ""{request.Body.Surname}""
            }}", null, "application/json");

            httpRequest.Content = jsonString;
            response = await httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            client.Name = request.Body.Name;
            client.Surname = request.Body.Surname;
            client.Phone = request.Body.Phone;

            await dbContext.SaveChangesAsync(cancellationToken);

            return "Данные обновлены!";
        }
    }
}

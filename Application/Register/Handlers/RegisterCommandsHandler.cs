using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Clients.Commands;
using Application.Clients;
using Domain;
using MediatR;
using Application.Register.Commands;
using System.Text.Json;
using Domain.Entities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Application.Register.Handlers
{
    public class Token
    {
        public required string access_token { get; set; }
    }

    public class UserId
    {
        [JsonProperty("id")]
        public required Guid id { get; set; }
    }

    internal class RegisterCommandsHandler(ApplicationDbContext dbContext, IClientMapper clientMapper, IClientService clientService) :
        IRequestHandler<RegisterUserCommand, CreatedOrUpdatedEntityViewModel<Guid>>
    {
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
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


            httpRequest = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/admin/realms/SportSchool/users");
            httpRequest.Headers.Add("Authorization", $"Bearer {token!.access_token}");

            
            var httpContent = new StringContent("{\r\n\"email\": " + $"\"{request.Body.Email}\"," +
                "\r\n\"firstName\": " + $"\"{request.Body.Name}\"," +
                "\r\n\"lastName\": " + $"\"{request.Body.Surname}\"," +
                "\r\n\"password\": " + $"\"{request.Body.Password}\"," +
                "\r\n\"enabled\": true," +
                "\r\n\"username\": " + $"\"{request.Body.Email}\"" + "\r\n}\r\n\r\n", null, "application/json");

            var jsonString = new StringContent($@"
            {{
                ""email"": ""{request.Body.Email}"",
                ""firstName"": ""{request.Body.Name}"",
                ""lastName"": ""{request.Body.Surname}"",
                ""enabled"": ""true"",
                ""username"": ""{request.Body.Email}"",
                ""credentials"": [{{
                    ""type"": ""password"",
                    ""value"": ""{request.Body.Password}"",
                    ""temporary"": ""false""
                }}]
            }}", null, "application/json");


            httpRequest.Content = jsonString;
            response = await httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();


            var getUserRequest = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:8080/admin/realms/SportSchool/users?username={request.Body.Email}");
            getUserRequest.Headers.Add("Authorization", $"Bearer {token.access_token}");

            response = await httpClient.SendAsync(getUserRequest);
            response.EnsureSuccessStatusCode();

            var userPorifle = await response.Content.ReadAsStringAsync();

            JArray jsonArray = JArray.Parse(userPorifle);

            JObject firstObject = (JObject)jsonArray[0];

            string userId = firstObject["id"]!.ToString();

            var clientToCreate = clientMapper.MapToEntity((request.Body.Name, request.Body.Surname, request.Body.Phone, Guid.Parse(userId)));
            var createdClient = await dbContext.AddAsync(clientToCreate, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            if (request.Body.Image != null)
            {
                var imagesDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.ToString() ?? string.Empty, "SportSchool", "wwwroot", "users");
                var filePath = Path.Combine(imagesDirectory, $"{createdClient.Entity.ExternalId.ToString()}.jpeg");
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    request.Body.Image.CopyTo(fileStream);
                }
            }

            return new CreatedOrUpdatedEntityViewModel(createdClient.Entity.Id);
        }
    }
}

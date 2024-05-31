using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Coachs.Commands;
using Application.Register.Handlers;
using Core.Exceptions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Application.Coachs.Handlers
{
    internal class CoachCommandsHandlers(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, ICoachMapper coachMapper, 
        ICoachService coachService, IClientService clientService) :
        IRequestHandler<CreateCoachCommand, CreatedOrUpdatedEntityViewModel<Guid>>, IRequestHandler<UpdateCoachCommand, string>
    {
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
        {
            var clientWithSameId = await clientService.GetClientAsync(request.Body.ExternalId.ToString(), false, cancellationToken);

            var coachWithSameId = await dbContext.Coachs
                .Where(x => x.ExternalId == request.Body.ExternalId)
                .SingleOrDefaultAsync(cancellationToken);

            if (coachWithSameId != null)
            {
                throw new BusinessLogicException(
                    $"Тренер с идентификатором \"{coachWithSameId.ExternalId}\" уже существует!");
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


            var addRoleRequest = new HttpRequestMessage(HttpMethod.Post, 
                $"http://localhost:8080/admin/realms/SportSchool/users/{clientWithSameId.ExternalId}/role-mappings/realm");
            addRoleRequest.Headers.Add("Authorization", $"Bearer {token!.access_token}");
            var addRoleContent = new StringContent($@"
            [
                {{
                ""id"": ""72ae3bc3-eebb-4148-8584-66888e3681ad"",
                ""name"": ""Coach""
                }}
            ]", null, "application/json");
            addRoleRequest.Content = addRoleContent;

            response = await httpClient.SendAsync(addRoleRequest);
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();


            var coachToCreate = coachMapper.MapToEntity((request.Body, clientWithSameId.Name, clientWithSameId.Surname, clientWithSameId.Phone));
            var createdCoach = await dbContext.AddAsync(coachToCreate, cancellationToken);

            dbContext.Remove(clientWithSameId);

            await dbContext.SaveChangesAsync();

            return new CreatedOrUpdatedEntityViewModel(createdCoach.Entity.Id);
        }

        public async Task<string> Handle(UpdateCoachCommand request, CancellationToken cancellationToken)
        {
            var coach = await coachService.GetCoachAync(contextAccessor.IdentityUserId, false, cancellationToken);

            coach.Name = request.Body.Name;
            coach.Surname = request.Body.Surname;
            coach.Phone = request.Body.Phone;
            coach.Institution = request.Body.Institution;
            coach.Faculty = request.Body.Faculty;
            coach.Speciality = request.Body.Speciality;
            coach.EducationForm = request.Body.EducationForm;
            coach.Qualification = request.Body.Qualification;
            coach.Job = request.Body.Job;
            coach.JobTitle = request.Body.JobTitle;
            coach.JobPeriod = request.Body.JobPeriod;

            await dbContext.SaveChangesAsync(cancellationToken);

            if (request.Body.Image != null)
            {
                var imagesDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.ToString() ?? string.Empty, "SportSchool", "wwwroot", "users");
                var filePath = Path.Combine(imagesDirectory, $"{coach.ExternalId}.jpeg");

                File.Delete(filePath);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    request.Body.Image.CopyTo(fileStream);
                }
            }

            return "Данные обновлены!";
        }
    }
}

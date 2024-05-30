using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Coachs.Commands;
using Core.Exceptions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

using Abstractions.CommonModels;
using Application.Events.SectionEvents.Commands;
using Core.Exceptions;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Events.SectionEvents.Handlers
{
    internal class SectionEventCommandsHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor) :
        IRequestHandler<CreateSectionEventCommand, CreatedOrUpdatedEntityViewModel<Guid>>
    {
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> Handle(CreateSectionEventCommand request, CancellationToken cancellationToken)
        {
            if (!contextAccessor.UserRoles.Contains("Coach"))
            {
                throw new ForbiddenException(
                    "Только тренер может добавлять секции!");
            }

            var coach = await dbContext.Coachs
                .Where(x => x.ExternalId == Guid.Parse(contextAccessor.IdentityUserId))
                .SingleOrDefaultAsync(cancellationToken);

            if (coach == null)
            {
                throw new ObjectNotFoundException(
                    $"Тренер с внешним идентификатором \"{contextAccessor.IdentityUserId}\" не найден!");
            }

            var section = await dbContext.Sections
                .Where(x => x.Id == request.Body.SectionId)
                .SingleOrDefaultAsync(cancellationToken);

            if (section == null)
            {
                throw new ObjectNotFoundException(
                    $"Секция с идентификатором \"{request.Body.SectionId}\" не найдена!");
            }

            if (section.CoachId != coach.Id)
            {
                throw new ForbiddenException(
                    $"Вы не можете добавлять занятия для этой секции, так как не являетесь ее тренером!");
            }

            var startTime = TimeOnly.Parse(request.Body.StartTime);
            var endTime = TimeOnly.Parse(request.Body.EndTime);
            var period = DateOnly.ParseExact(request.Body.Period, "dd-MM-yyyy");

            var sectionEventWithSameData = await dbContext.SectionEvents
                .Where(x => x.DayOfWeek == request.Body.DayOfWeek)
                .Where(x => x.StartTime == startTime)
                .Where(x => x.EndTime == endTime)
                .Where(x => x.Period == period)
                .Where(x => x.SectionId == request.Body.SectionId)
                .SingleOrDefaultAsync(cancellationToken);

            if (sectionEventWithSameData != null)
            {
                throw new BusinessLogicException(
                    $"Занятие с указанными данными уже существует!");
            }



            var sectionEventToCreate = new SectionEvent
            {
                DayOfWeek = request.Body.DayOfWeek,
                StartTime = startTime,
                EndTime = endTime,
                Period = period,
                SectionId = request.Body.SectionId
            };

            var createdSectionEvent = await dbContext.AddAsync(sectionEventToCreate, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreatedOrUpdatedEntityViewModel(createdSectionEvent.Entity.Id);
        }
    }
}

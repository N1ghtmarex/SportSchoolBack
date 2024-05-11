using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Events.IndividualEvents.Commands;
using Core.Exceptions;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Application.Events.IndividualEvents.Handlers
{
    public class IndividualEventCommandsHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, IClientService clientService, ICoachService coachService,
        ISportService sportService, IRoomService roomService) :
        IRequestHandler<CreateIndividualEventCommand, CreatedOrUpdatedEntityViewModel<Guid>>, IRequestHandler<AddClientToIndividualEventCommand>,
        IRequestHandler<DeleteClientFromIndividualEventCommand>
    {
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> Handle(CreateIndividualEventCommand request, CancellationToken cancellationToken)
        {
            if (!contextAccessor.UserRoles.Contains("Coach"))
            {
                throw new ForbiddenException(
                    "Только тренер может добавлять индивидуальные занятия!");
            }

            var coach = await coachService.GetCoachAync(contextAccessor.IdentityUserId, cancellationToken);

            var sport = await sportService.GetSportAsync(request.Body.SportId, cancellationToken);

            var room = await roomService.GetRoomAsync(request.Body.RoomId, cancellationToken);

            var startDate = DateTime.ParseExact(request.Body.StartDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture).ToUniversalTime();
            var duration = TimeOnly.Parse(request.Body.Duration);
            var endDate = startDate.AddHours(duration.Hour).AddMinutes(duration.Minute);

            var individualEventWithSameData = await dbContext.IndividualEvents
                .AsNoTracking()
                .Where(x => x.SportId == request.Body.SportId)
                .Where(x => x.CoachId == coach.Id)
                .Where(x => x.SportId == request.Body.SportId)
                .Where(x => x.RoomId == request.Body.RoomId)
                .Where(x => x.StartDate == startDate)
                .Where(x => x.EndDate == endDate)
                .SingleOrDefaultAsync(cancellationToken);

            if (individualEventWithSameData != null)
            {
                throw new BusinessLogicException(
                    $"Индивидуальное занятие с такими данными уже существует!");
            }

            var individualEventToCreate = new IndividualEvent
            {
                StartDate = startDate,
                EndDate = endDate,
                SportId = sport.Id,
                CoachId = coach.Id,
                RoomId = room.Id
            };

            var createdindividualEvent = await dbContext.IndividualEvents.AddAsync(individualEventToCreate, cancellationToken);
            await dbContext.SaveChangesAsync();

            return new CreatedOrUpdatedEntityViewModel(createdindividualEvent.Entity.Id);
        }

        public async Task Handle(AddClientToIndividualEventCommand request, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientAsync(contextAccessor.IdentityUserId, false, cancellationToken);

            var individualEvent = await dbContext.IndividualEvents
                .Where(x => x.Id == request.EventId)
                .SingleOrDefaultAsync(cancellationToken);

            if (individualEvent == null)
            {
                throw new ObjectNotFoundException(
                    $"Индивидуальное занятие с идентификатором {request.EventId} не найдено!");
            }

            if (individualEvent.ClientId != null)
            {
                throw new BusinessLogicException(
                    $"На это занятие уже записан другой клиент!");
            }

            individualEvent.ClientId = client.Id;
            await dbContext.SaveChangesAsync();
        }

        public async Task Handle(DeleteClientFromIndividualEventCommand request, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientAsync(contextAccessor.IdentityUserId, false, cancellationToken);

            var individualEvent = await dbContext.IndividualEvents
                .Where(x => x.Id == request.EventId)
                .SingleOrDefaultAsync(cancellationToken);

            if (individualEvent == null)
            {
                throw new ObjectNotFoundException(
                    $"Индивидуальное занятие с идентификатором {request.EventId} не найдено!");
            }

            if (individualEvent.ClientId != client.Id)
            {
                throw new BusinessLogicException(
                    $"Вы не записаны на это занятие!");
            }

            individualEvent.ClientId = null;
            await dbContext.SaveChangesAsync();
        }
    }
}

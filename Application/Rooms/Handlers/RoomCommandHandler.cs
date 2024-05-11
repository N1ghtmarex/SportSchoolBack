using Abstractions.CommonModels;
using Application.Rooms.Commands;
using Core.Exceptions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rooms.Handlers
{
    internal class RoomCommandHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, IRoomMapper roomMapper) :
        IRequestHandler<CreateRoomCommand, CreatedOrUpdatedEntityViewModel<Guid>>
    {
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            if (!contextAccessor.UserRoles.Contains("Coach"))
            {
                throw new ForbiddenException(
                    "Только тренер может добавлять залы!");
            }

            var roomWithSameName = await dbContext.Rooms
                .Where(x => x.Name == request.Body.Name)
                .SingleOrDefaultAsync();

            if (roomWithSameName != null) 
            {
                throw new BusinessLogicException(
                    $"Зал с названием {roomWithSameName.Name} уже существует!");
            }

            var roomToCreate = roomMapper.MapToEntity(request.Body);
            var createdRoom = await dbContext.AddAsync(roomToCreate, cancellationToken);
            await dbContext.SaveChangesAsync();

            return new CreatedOrUpdatedEntityViewModel(createdRoom.Entity.Id);
        }
    }
}

using Abstractions.CommonModels;
using Application.Sports.Commands;
using Core.Exceptions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Sports.Handlers
{
    internal class SportCommandsHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, ISportMapper sportMapper) :
        IRequestHandler<CreateSportCommand, CreatedOrUpdatedEntityViewModel<Guid>>
    {
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> Handle(CreateSportCommand request, CancellationToken cancellationToken)
        {
            if (!contextAccessor.UserRoles.Contains("Coach"))
            {
                throw new ForbiddenException(
                    "Только тренер может добавлять новый вид спорта!");
            }

            var sportWithSameName = await dbContext.Sports
                .Where(x => x.Name == request.Body.Name)
                .SingleOrDefaultAsync();

            if (sportWithSameName != null)
            {
                throw new BusinessLogicException(
                    $"Спорт с названием \"{sportWithSameName.Name}\" уже существует!");
            }

            var sportToCreate = sportMapper.MapToEntity(request.Body);
            var createdSport = await dbContext.AddAsync(sportToCreate, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreatedOrUpdatedEntityViewModel(createdSport.Entity.Id);
        }
    }
}

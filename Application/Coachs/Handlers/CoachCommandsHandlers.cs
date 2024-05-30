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
        IRequestHandler<CreateCoachCommand, CreatedOrUpdatedEntityViewModel<Guid>>
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
    }
}

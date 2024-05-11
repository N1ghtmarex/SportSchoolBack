using Abstractions.CommonModels;
using Application.Clients;
using Application.Clients.Commands;
using Core.Exceptions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Handlers
{
    internal class ClientCommandsHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, IClientMapper clientMapper) :
        IRequestHandler<CreateClientCommand, CreatedOrUpdatedEntityViewModel<Guid>>
    {
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var clientWithSameId = await dbContext.Clients
                .Where(x => x.ExternalId == Guid.Parse(contextAccessor.IdentityUserId))
                .SingleOrDefaultAsync(cancellationToken);

            if (clientWithSameId != null)
            {
                throw new BusinessLogicException(
                    $"Пользователь с идентификатором \"{clientWithSameId.ExternalId}\" уже существует!");
            }
            
            var clientToCreate = clientMapper.MapToEntity((contextAccessor.UserName, contextAccessor.UserSurname, Guid.Parse(contextAccessor.IdentityUserId)));
            var createdClient = await dbContext.AddAsync(clientToCreate, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreatedOrUpdatedEntityViewModel(createdClient.Entity.Id);
        }
    }
}

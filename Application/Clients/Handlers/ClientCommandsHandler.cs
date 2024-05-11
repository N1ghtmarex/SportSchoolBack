using Abstractions.CommonModels;
using Abstractions.Services;
using Application.Clients;
using Application.Clients.Commands;
using Core.Exceptions;
using Domain;
using MediatR;

namespace Application.Users.Handlers
{
    internal class ClientCommandsHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, IClientMapper clientMapper, IClientService clientService) :
        IRequestHandler<CreateClientCommand, CreatedOrUpdatedEntityViewModel<Guid>>
    {
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var clientWithSameId = await clientService.GetClientAsync(contextAccessor.IdentityUserId, false, cancellationToken);

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

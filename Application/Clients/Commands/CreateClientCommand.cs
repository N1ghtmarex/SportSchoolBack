using Abstractions.CommonModels;
using MediatR;

namespace Application.Clients.Commands
{
    public class CreateClientCommand : IRequest<CreatedOrUpdatedEntityViewModel<Guid>>
    {
    }
}

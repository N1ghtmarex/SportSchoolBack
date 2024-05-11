using Abstractions.CommonModels;
using MediatR;

namespace Application.Coachs.Commands
{
    public class CreateCoachCommand : IRequest<CreatedOrUpdatedEntityViewModel<Guid>>
    {
    }
}

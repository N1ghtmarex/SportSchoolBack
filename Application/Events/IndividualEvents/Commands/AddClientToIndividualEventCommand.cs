using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Events.IndividualEvents.Commands
{
    public class AddClientToIndividualEventCommand : IRequest
    {
        [FromRoute]
        public required Guid EventId { get; set; }
    }
}

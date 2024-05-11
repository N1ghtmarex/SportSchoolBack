using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Events.IndividualEvents.Commands
{
    public class DeleteClientFromIndividualEventCommand : IRequest
    {
        [FromRoute]
        public required Guid EventId { get; set; }
    }
}

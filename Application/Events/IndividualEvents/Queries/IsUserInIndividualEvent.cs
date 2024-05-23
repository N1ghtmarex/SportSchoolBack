using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Events.IndividualEvents.Queries
{
    public class IsUserInIndividualEvent : IRequest<bool>
    {
        [FromRoute]
        public required Guid IndividualEventId {  get; set; }
    }
}

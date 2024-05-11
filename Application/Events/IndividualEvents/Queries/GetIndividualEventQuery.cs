using Application.Events.IndividualEvents.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Events.IndividualEvents.Queries
{
    public class GetIndividualEventQuery : IRequest<IndividualEventListViewModel>
    {
        [FromRoute]
        public required Guid IndividualEventId { get; set; }
    }
}

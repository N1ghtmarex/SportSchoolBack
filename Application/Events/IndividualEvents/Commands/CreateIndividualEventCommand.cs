using Abstractions.CommonModels;
using Application.Events.IndividualEvents.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Events.IndividualEvents.Commands
{
    public class CreateIndividualEventCommand : IRequest<CreatedOrUpdatedEntityViewModel<Guid>>
    {
        [FromBody]
        public required CreateIndividualEventModel Body { get; set; }
    }
}

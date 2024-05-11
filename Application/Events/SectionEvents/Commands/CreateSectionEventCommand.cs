using Abstractions.CommonModels;
using Application.Events.SectionEvents.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Events.SectionEvents.Commands
{
    public class CreateSectionEventCommand: IRequest<CreatedOrUpdatedEntityViewModel<Guid>>
    {
        [FromBody]
        public required CreateSectionEventModel Body { get; set; }
    }
}

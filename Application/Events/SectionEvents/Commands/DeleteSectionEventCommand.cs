using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Events.SectionEvents.Commands
{
    public class DeleteSectionEventCommand : IRequest<string>
    {
        [FromRoute]
        public required Guid EventId { get; set; }
    }
}

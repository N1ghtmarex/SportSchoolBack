using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Sections.Commands
{
    public class DeleteSectionCommand: IRequest
    {
        [FromRoute]
        public required Guid SectionId { get; set; }
    }
}

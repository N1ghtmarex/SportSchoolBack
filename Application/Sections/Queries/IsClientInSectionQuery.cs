using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Sections.Queries
{
    public class IsClientInSectionQuery : IRequest<bool>
    {
        [FromRoute]
        public required Guid SectionId { get; set; }
    }
}

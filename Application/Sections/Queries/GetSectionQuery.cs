using Application.Sections.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Sections.Queries
{
    public class GetSectionQuery : IRequest<SectionListViewModel>
    {
        [FromRoute]
        public required Guid SectionId { get; set; }
    }
}

using Application.Sections.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Sections.Commands
{
    public class RemoveClientFromSectionCommand : IRequest
    {
        [FromBody]
        public required RemoveClientFromSectionModel Body { get; set; }
    }
}

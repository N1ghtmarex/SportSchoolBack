using Application.Sections.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Sections.Commands
{
    public class AddClientToSectionCommand : IRequest
    {
        [FromBody]
        public required AddClientToSectionModel Body { get; set; }
    }
}

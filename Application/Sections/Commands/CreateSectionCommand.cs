using Abstractions.CommonModels;
using Application.Sections.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Sections.Commands
{
    public class CreateSectionCommand : IRequest<CreatedOrUpdatedEntityViewModel<Guid>>
    {
        [FromForm]
        public required CreateSectionModel Body { get; set; }
    }
}

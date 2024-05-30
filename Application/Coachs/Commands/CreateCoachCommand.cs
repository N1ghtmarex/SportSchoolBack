using Abstractions.CommonModels;
using Application.Coachs.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Coachs.Commands
{
    public class CreateCoachCommand : IRequest<CreatedOrUpdatedEntityViewModel<Guid>>
    {
        [FromBody]
        public required CreateCoachModel Body { get; set; }
    }
}

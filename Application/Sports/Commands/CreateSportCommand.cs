using Abstractions.CommonModels;
using Application.Sports.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Sports.Commands
{
    public class CreateSportCommand : IRequest<CreatedOrUpdatedEntityViewModel<Guid>>
    {
        [FromBody]
        public required CreateSportModel Body { get; set; }
    }
}

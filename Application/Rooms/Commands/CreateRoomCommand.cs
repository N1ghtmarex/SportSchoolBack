using Abstractions.CommonModels;
using Application.Rooms.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Rooms.Commands
{
    public class CreateRoomCommand : IRequest<CreatedOrUpdatedEntityViewModel<Guid>>
    {
        [FromBody]
        public required CreateRoomModel Body { get; set; }
    }
}

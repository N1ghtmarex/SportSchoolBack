using Application.Clients.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Clients.Commands
{
    public class UpdateClientCommand: IRequest<string>
    {
        [FromForm]
        public required UpdateClientModel Body { get; set; }
    }
}

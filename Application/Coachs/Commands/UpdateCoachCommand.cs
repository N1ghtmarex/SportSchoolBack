using Application.Coachs.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Coachs.Commands
{
    public class UpdateCoachCommand : IRequest<string>
    {
        [FromForm]
        public required UpdateCoachModel Body { get; set; }
    }
}

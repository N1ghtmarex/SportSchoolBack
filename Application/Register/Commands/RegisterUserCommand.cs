using Abstractions.CommonModels;
using Application.Register.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Register.Commands
{
    public class RegisterUserCommand : IRequest<CreatedOrUpdatedEntityViewModel<Guid>>
    {
        [FromForm]
        public required RegisterUserModel Body { get; set; }
    }
}

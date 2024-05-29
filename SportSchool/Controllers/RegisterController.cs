using Abstractions.CommonModels;
using Application.Register.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SportSchool.Controllers
{
    [Route("api/admin/register")]
    [AllowAnonymous]
    public class RegisterController(ISender sender) : BaseController
    {

        [HttpPost]
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> RegisterUser([FromForm] RegisterUserCommand command, CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }
    }
}

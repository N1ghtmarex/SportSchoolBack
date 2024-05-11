using Abstractions.CommonModels;
using Application.Clients.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSchool.StartupConfigurations;

namespace SportSchool.Controllers
{
    [Route("api/admin/users")]
    [Authorize(Policy = KeycloakAuthConfiguration.AdminApiPolicy)]
    [ApiExplorerSettings(GroupName = "sportschool")]
    public class UserController(ISender sender) : BaseController
    {
        [HttpPost]
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> CreateUser(CreateClientCommand command, CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }
    }
}

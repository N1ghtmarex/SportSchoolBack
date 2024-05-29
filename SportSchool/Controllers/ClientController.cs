using Application.Clients.Dtos;
using Application.Clients.Queries;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSchool.StartupConfigurations;

namespace SportSchool.Controllers
{
    [Route("api/admin/clients")]
    [Authorize(Policy = KeycloakAuthConfiguration.AdminApiPolicy)]
    [ApiExplorerSettings(GroupName = "sportschool")]
    public class ClientController(ISender sender) : BaseController
    {
        [HttpGet("{ClientId}")]
        public async Task<ClientViewModel> GetClient([FromQuery] GetClientQuery query, CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }

        [HttpGet]
        public async Task<PagedResult<ClientListViewModel>> GetClients([FromQuery] GetClientsListQuery query, CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }
    }
}

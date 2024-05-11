using Abstractions.CommonModels;
using Application.Sports.Commands;
using Application.Sports.Dtos;
using Application.Sports.Queries;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSchool.StartupConfigurations;

namespace SportSchool.Controllers
{
    [Route("api/admin/sports")]
    [Authorize(Policy = KeycloakAuthConfiguration.AdminApiPolicy)]
    [ApiExplorerSettings(GroupName = "sportschool")]
    public class SportController(ISender sender) : BaseController
    {
        [HttpPost]
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> CreateSport([FromQuery] CreateSportCommand command, CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }

        [HttpGet]
        public async Task<PagedResult<SportListViewModel>> GetSportsList(
        [FromQuery] GetSportsListQuery query, CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }
    }
}

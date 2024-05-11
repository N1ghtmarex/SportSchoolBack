using Abstractions.CommonModels;
using Application.Events.IndividualEvents.Commands;
using Application.Events.IndividualEvents.Dtos;
using Application.Events.IndividualEvents.Queries;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSchool.StartupConfigurations;

namespace SportSchool.Controllers
{
    [Route("api/admin/individual-event")]
    [Authorize(Policy = KeycloakAuthConfiguration.AdminApiPolicy)]
    [ApiExplorerSettings(GroupName = "sportschool")]
    public class IndividualEventController(ISender sender) : BaseController
    {
        [HttpPost]
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> CreateIndividualEvent([FromQuery] CreateIndividualEventCommand command,
            CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }

        [HttpPost("enter/{EventId}")]
        public async Task<ActionResult> AddClientToIndividualEvent([FromQuery] AddClientToIndividualEventCommand command,
            CancellationToken cancellationToken)
        {
            await sender.Send(command, cancellationToken);
            return Ok();
        }

        [HttpPost("leave/{EventId}")]
        public async Task<ActionResult> DeleteClientFromIndividualEvent([FromQuery] DeleteClientFromIndividualEventCommand command,
            CancellationToken cancellationToken)
        {
            await sender.Send(command, cancellationToken); 
            return Ok();
        }

        [HttpGet("filter={Filter}")]
        public async Task<PagedResult<IndividualEventListViewModel>> GetIndividualEventsList([FromQuery] GetIndividualEventsListQuery query,
            CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }

        [HttpGet("{IndividualEventId}")]
        public async Task<IndividualEventListViewModel> GetIndividualEvent([FromQuery] GetIndividualEventQuery query,
            CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }

        [HttpGet("my")]
        public async Task<PagedResult<IndividualEventListViewModel>> GetClientsIndividualEventsList([FromQuery] GetClientsIndividualEventsListQuery query,
            CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }
    }
}

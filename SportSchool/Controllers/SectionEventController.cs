using Abstractions.CommonModels;
using Application.Events.SectionEvents.Commands;
using Application.Events.SectionEvents.Dtos;
using Application.Events.SectionEvents.Queries;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSchool.StartupConfigurations;

namespace SportSchool.Controllers
{
    [Route("api/admin/section-event")]
    [Authorize(Policy = KeycloakAuthConfiguration.AdminApiPolicy)]
    [ApiExplorerSettings(GroupName = "sportschool")]
    public class SectionEventController(ISender sender) : BaseController
    {
        [HttpPost]
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> CreateSectionEvent([FromQuery] CreateSectionEventCommand command,
            CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }

        [HttpGet]
        public async Task<PagedResult<SectionEventListViewModel>> GetSectionEventsList(
            [FromQuery] GetSectionEventsListQuery query, CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }

        [HttpGet("{SectionId}")]
        public async Task<PagedResult<SectionEventListViewModel>> GetSectionEvent(
            [FromQuery] GetSectionEventQuery query, CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }
    }
}

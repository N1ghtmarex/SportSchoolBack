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
        /// <summary>
        /// Добавить занятие секции
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> CreateSectionEvent([FromQuery] CreateSectionEventCommand command,
            CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }

        /// <summary>
        /// Получить список занятий всех секций
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResult<SectionEventListViewModel>> GetSectionEventsList(
            [FromQuery] GetSectionEventsListQuery query, CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }

        /// <summary>
        /// Получить список занятий конкретной секции
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{SectionId}")]
        public async Task<PagedResult<SectionEventListViewModel>> GetSectionEvent(
            [FromQuery] GetSectionEventQuery query, CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }

        [HttpDelete("{EventId}")]
        public async Task<ActionResult<string>> DeleteEvent(
            [FromQuery] DeleteSectionEventCommand command, CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }
    }
}

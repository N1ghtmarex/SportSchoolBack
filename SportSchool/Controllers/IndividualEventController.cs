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
        /// <summary>
        /// Добавить индивидуальное занятие
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CreatedOrUpdatedEntityViewModel<Guid>> CreateIndividualEvent([FromQuery] CreateIndividualEventCommand command,
            CancellationToken cancellationToken)
        {
            return await sender.Send(command, cancellationToken);
        }

        /// <summary>
        /// Добавить пользователя в индивидуальное занятие
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("enter/{EventId}")]
        public async Task<ActionResult> AddClientToIndividualEvent([FromQuery] AddClientToIndividualEventCommand command,
            CancellationToken cancellationToken)
        {
            await sender.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Удалить пользователя из индивидуального занятия
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("leave/{EventId}")]
        public async Task<ActionResult> DeleteClientFromIndividualEvent([FromQuery] DeleteClientFromIndividualEventCommand command,
            CancellationToken cancellationToken)
        {
            await sender.Send(command, cancellationToken); 
            return Ok();
        }

        /// <summary>
        /// Получить список индивидуальных занятий
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("filter={Filter}")]
        public async Task<PagedResult<IndividualEventListViewModel>> GetIndividualEventsList([FromQuery] GetIndividualEventsListQuery query,
            CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }

        /// <summary>
        /// Получить индивидуальное занятие
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{IndividualEventId}")]
        public async Task<IndividualEventListViewModel> GetIndividualEvent([FromQuery] GetIndividualEventQuery query,
            CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }

        /// <summary>
        /// Получить список индивидуальных занятий, на которые записан пользователь
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("my")]
        public async Task<PagedResult<IndividualEventListViewModel>> GetClientsIndividualEventsList([FromQuery] GetClientsIndividualEventsListQuery query,
            CancellationToken cancellationToken)
        {
            return await sender.Send(query, cancellationToken);
        }
    }
}

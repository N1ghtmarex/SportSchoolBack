using Abstractions.CommonModels;
using Application.Sections.Commands;
using Application.Sections.Dtos;
using Application.Sections.Queries;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSchool.StartupConfigurations;

namespace SportSchool.Controllers;

[Route("api/admin/sections")]
[Authorize(Policy = KeycloakAuthConfiguration.AdminApiPolicy)]
[ApiExplorerSettings(GroupName = "sportschool")]
public class SectionController(ISender sender) : BaseController
{
    /// <summary>
    /// Создать секцию
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<CreatedOrUpdatedEntityViewModel<Guid>> CreateSection([FromForm] CreateSectionCommand command, CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }

    /// <summary>
    /// Получить список секций
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResult<SectionListViewModel>> GetSectionsList(
        [FromQuery] GetSectionsListQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    /// <summary>
    /// Получить секцию по идентификатору
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{SectionId}")]
    public async Task<SectionListViewModel> GetSection([FromQuery] GetSectionQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    /// <summary>
    /// Добавить пользователя в секцию
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("enter")]
    public async Task<ActionResult> AddClientToSection([FromQuery] AddClientToSectionCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Получить список секций, в которые вступил пользватель
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("entered")]
    public async Task<PagedResult<SectionListViewModel>> GetUserSectionsList(
        [FromQuery] GetUserSectionsQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    /// <summary>
    /// Удалить пользователя из секции
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("leave")]
    public async Task<ActionResult> RemoveClientFromSection([FromQuery] RemoveClientFromSectionCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Удалить секцию
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("remove/{SectionId}")]
    public async Task<ActionResult> DeleteSection([FromQuery] DeleteSectionCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken); 
        return Ok();
    }

    /// <summary>
    /// Есть ли пользователь в секции
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("is-entered/{SectionId}")]
    public async Task<ActionResult<bool>> IsClientInSection([FromQuery] IsClientInSectionQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
}
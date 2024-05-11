using Abstractions.CommonModels;
using Application.Rooms.Commands;
using Application.Rooms.Dtos;
using Application.Rooms.Queries;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSchool.StartupConfigurations;

namespace SportSchool.Controllers;

[Route("api/admin/rooms")]
[Authorize(Policy = KeycloakAuthConfiguration.AdminApiPolicy)]
[ApiExplorerSettings(GroupName = "sportschool")]
public class RoomController(ISender sender) : BaseController
{
    /// <summary>
    /// Создать зал
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<CreatedOrUpdatedEntityViewModel<Guid>> CreateRoom([FromQuery] CreateRoomCommand command, CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }

    /// <summary>
    /// Получить список залов
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResult<RoomListViewModel>> GetRoomsListQuery(
        [FromQuery] GetRoomsListQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
}


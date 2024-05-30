using Abstractions.CommonModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportSchool.StartupConfigurations;
using Application.Coachs.Commands;
using Core.EntityFramework.Features.SearchPagination.Models;
using Application.Coachs.Dtos;
using Application.Coachs.Queries;

namespace SportSchool.Controllers;

[Route("api/admin/coachs")]
[Authorize(Policy = KeycloakAuthConfiguration.AdminApiPolicy)]
[ApiExplorerSettings(GroupName = "sportschool")]
public class CoachController(ISender sender) : BaseController
{
    [HttpPost]
    public async Task<CreatedOrUpdatedEntityViewModel<Guid>> CreateCoach([FromQuery] CreateCoachCommand command, CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }

    [HttpGet]
    public async Task<PagedResult<CoachListViewModel>> GetCoachsList(
        [FromQuery] GetCoachsListQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
}


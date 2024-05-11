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
    [HttpPost]
    public async Task<CreatedOrUpdatedEntityViewModel<Guid>> CreateSection([FromForm] CreateSectionCommand command, CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }

    [HttpGet]
    public async Task<PagedResult<SectionListViewModel>> GetSectionsList(
        [FromQuery] GetSectionsListQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    [HttpGet("{SectionId}")]
    public async Task<SectionListViewModel> GetSection([FromQuery] GetSectionQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    [HttpPost("enter")]
    public async Task<ActionResult> AddClientToSection([FromQuery] AddClientToSectionCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet("entered")]
    public async Task<PagedResult<SectionListViewModel>> GetUserSectionsList(
        [FromQuery] GetUserSectionsQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    [HttpPost("leave")]
    public async Task<ActionResult> RemoveClientFromSection([FromQuery] RemoveClientFromSectionCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete("remove/{SectionId}")]
    public async Task<ActionResult> DeleteSection([FromQuery] DeleteSectionCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken); 
        return Ok();
    }

    [HttpGet("is-entered/{SectionId}")]
    public async Task<ActionResult<bool>> IsClientInSection([FromQuery] IsClientInSectionQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
}
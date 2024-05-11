using Application.BaseModels;
using Application.Events.IndividualEvents.Dtos;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Events.IndividualEvents.Queries
{
    public class GetIndividualEventsListQuery : SearchablePagedQuery, IRequest<PagedResult<IndividualEventListViewModel>>
    {
        [FromRoute]
        public required string Filter { get; set; }
        public Guid? SportId { get; set; }
    }
}

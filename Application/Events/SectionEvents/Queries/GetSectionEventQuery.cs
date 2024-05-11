using Application.BaseModels;
using Application.Events.SectionEvents.Dtos;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Events.SectionEvents.Queries
{
    public class GetSectionEventQuery : SearchablePagedQuery, IRequest<PagedResult<SectionEventListViewModel>>
    {
        [FromRoute]
        public required Guid SectionId { get; set; }
    }
}

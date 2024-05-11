using Application.BaseModels;
using Application.Events.SectionEvents.Dtos;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;

namespace Application.Events.SectionEvents.Queries
{
    public class GetSectionEventsListQuery: SearchablePagedQuery, IRequest<PagedResult<SectionEventListViewModel>>
    {
    }
}

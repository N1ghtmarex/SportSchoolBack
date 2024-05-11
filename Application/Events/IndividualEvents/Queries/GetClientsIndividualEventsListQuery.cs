using Application.BaseModels;
using Application.Events.IndividualEvents.Dtos;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;

namespace Application.Events.IndividualEvents.Queries
{
    public class GetClientsIndividualEventsListQuery : SearchablePagedQuery, IRequest<PagedResult<IndividualEventListViewModel>>
    {
    }
}

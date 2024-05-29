using Application.BaseModels;
using Application.Clients.Dtos;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;

namespace Application.Clients.Queries
{
    public class GetClientsListQuery: SearchablePagedQuery, IRequest<PagedResult<ClientListViewModel>>
    {
    }
}

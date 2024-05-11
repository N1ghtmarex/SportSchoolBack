using Application.BaseModels;
using Application.Sports.Dtos;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;

namespace Application.Sports.Queries
{
    public class GetSportsListQuery: SearchablePagedQuery, IRequest<PagedResult<SportListViewModel>>
    {
    }
}

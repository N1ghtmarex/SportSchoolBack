using Application.BaseModels;
using Application.Coachs.Dtos;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;

namespace Application.Coachs.Queries
{
    public class GetCoachsListQuery: SearchablePagedQuery, IRequest<PagedResult<CoachListViewModel>>
    {
    }
}

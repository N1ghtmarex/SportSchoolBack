using Application.BaseModels;
using Application.Rooms.Dtos;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;

namespace Application.Rooms.Queries
{
    public class GetRoomsListQuery: SearchablePagedQuery, IRequest<PagedResult<RoomListViewModel>>
    {
    }
}

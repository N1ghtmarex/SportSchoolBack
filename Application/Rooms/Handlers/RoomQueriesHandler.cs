using Abstractions.CommonModels;
using Application.Rooms.Dtos;
using Application.Rooms.Queries;
using Core.EntityFramework.Features.SearchPagination;
using Core.EntityFramework.Features.SearchPagination.Models;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Rooms.Handlers
{
    internal class RoomQueriesHandler(ApplicationDbContext dbContext, ICurrentHttpContextAccessor contextAccessor, IRoomMapper roomMapper) :
        IRequestHandler<GetRoomsListQuery, PagedResult<RoomListViewModel>>, IRequestHandler<GetRoomQuery, RoomViewModel>
    {
        public async Task<PagedResult<RoomListViewModel>> Handle(GetRoomsListQuery request, CancellationToken cancellationToken)
        {
            var roomQuery = dbContext.Rooms
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ApplySearch(request, x => x.Name);

            var roomsList = await roomQuery
                .ApplyPagination(request)
            .ToListAsync(cancellationToken);

            var result = roomsList.Select(roomMapper.MapToListViewModel);
            return result.AsPagedResult(request, await roomQuery.CountAsync(cancellationToken));
        }

        public Task<RoomViewModel> Handle(GetRoomQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

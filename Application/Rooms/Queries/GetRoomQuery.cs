using Application.Rooms.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Rooms.Queries
{
    public class GetRoomQuery: IRequest<RoomViewModel>
    {
        [FromRoute]
        public required Guid RoomId { get; set; }
    }
}

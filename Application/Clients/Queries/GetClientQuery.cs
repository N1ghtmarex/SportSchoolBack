using Application.Clients.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Clients.Queries
{
    public class GetClientQuery: IRequest<ClientViewModel>
    {
        [FromRoute]
        public required Guid ClientId { get; set; }
    }
}

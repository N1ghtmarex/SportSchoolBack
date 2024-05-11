using Application.Sports.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Sports.Queries
{
    public class GetSportQuery : IRequest<SportViewModel>
    {
        [FromRoute]
        public required Guid SportId { get; set; }
    }
}

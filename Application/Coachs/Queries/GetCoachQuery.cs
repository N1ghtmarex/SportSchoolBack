using Application.Coachs.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Coachs.Queries
{
    public class GetCoachQuery : IRequest<CoachViewModel>
    {
        [FromRoute]
        public required Guid CoachId { get; set; }
    }
}

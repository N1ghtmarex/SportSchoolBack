using Application.BaseModels;
using Application.Sections.Dtos;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Sections.Queries
{
    public class GetUserSectionsQuery : SearchablePagedQuery, IRequest<PagedResult<SectionListViewModel>>
    {
        [FromQuery]
        public Guid? SportId { get; set; }
    }
}

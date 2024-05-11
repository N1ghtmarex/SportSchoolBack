using Application.BaseModels;
using Application.Sections.Dtos;
using Core.EntityFramework.Features.SearchPagination.Models;
using MediatR;

namespace Application.Sections.Queries
{
    public class GetUserSectionsQuery : SearchablePagedQuery, IRequest<PagedResult<SectionListViewModel>>
    {
    }
}

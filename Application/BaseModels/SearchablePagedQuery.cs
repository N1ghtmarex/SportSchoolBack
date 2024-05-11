using Core.EntityFramework.Features.SearchPagination.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.BaseModels
{
    public class SearchablePagedQuery : PagedQuery, ISearchQuery
    {
        /// <summary>
        /// Строка поиска.
        /// </summary>
        [FromQuery]
        public string? SearchQuery { get; set; }
    }
}

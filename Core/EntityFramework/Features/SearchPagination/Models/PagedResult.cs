namespace Core.EntityFramework.Features.SearchPagination.Models
{
    public class PagedResult<T>
    {
        public long TotalItems { get; set; }
        public long ItemsQuantity { get; set; }
        public int ItemsOffset { get; set; }
        public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
    }
}

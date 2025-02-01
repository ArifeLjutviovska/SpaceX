namespace SpacexServer.Api.Common.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }

}

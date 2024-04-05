namespace Core.Options.Pagination
{
    public class PagedList<T>
    {
        private PagedList(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
        public List<T> Items { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;
        public int TotalPages => Convert.ToInt32(Math.Ceiling((double)TotalCount / PageSize));

        public static PagedList<T> Create(IEnumerable<T> collection, PaginationOptions paginationOptions)
        {
            var totalCount = collection.Count();
            var items = collection.Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize).Take(paginationOptions.PageSize).ToList();
            return new PagedList<T>(items, paginationOptions.PageNumber, paginationOptions.PageSize, totalCount);
        }

    }
}

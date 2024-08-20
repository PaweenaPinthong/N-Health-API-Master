namespace N_Health_API.Models.Shared
{
    public class PaginatedListModel<T>
    {
        public List<T> ListItem { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedListModel(List<T> items, int count, int pageNumber, int pageSize)
        {
            ListItem = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}

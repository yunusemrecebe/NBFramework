namespace Core.Utilities.Results
{
    public class PagedListResult<T> : IResult, IPagedListResult<T>
    {
        public PagedListResult(List<T> data, int pageNumber, int pageSize, int totalPageCount, int totalRecordCount)
        {
            Data = data;
            Message = "";
            Success = true;
            PageNumber = pageNumber <= 0 ? 1 : pageNumber;
            PageSize = pageSize <= 0 ? 1 : pageSize;
            TotalPageCount = totalPageCount <= 0 ? 1 : totalPageCount;
            TotalRecordCount = totalRecordCount <= 0 ? 1 : totalRecordCount;
        }

        public List<T> Data { get; }
        public string Message { get; }
        public bool Success { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPageCount { get; set; }
        public int TotalRecordCount { get; set; }

    }
}

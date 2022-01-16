namespace Core.Utilities.Results
{
    public interface IPagedListResult<T> : IResult
    {
        public List<T> Data { get; }
        public int TotalRecordCount { get; }
    }
}

using Core.Utilities.Results;
using System.Linq.Expressions;

namespace DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        T Get(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        T GetFirst(Expression<Func<T, bool>> expression);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetList();
        IEnumerable<T> GetList(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetListAsync();
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression);
        PagedListResult<T> GetPagedList(int pageNumber, int pageSize, string orderBy, bool orderByDesc = true);
        PagedListResult<T> GetPagedList(Expression<Func<T, bool>> expression, int pageNumber, int pageSize, string orderBy, bool orderByDesc = true);
        int Commit();
        Task<int> CommitAsync();
        IQueryable<T> GetBaseQuery(bool noTracking = true);
        Task<int> Execute(FormattableString interpolatedQueryString);
        Result InTransaction(Func<Result> transaction, Action? successAction, Action<Exception>? exceptionAction);
        int GetCount(Expression<Func<T, bool>>? expression);
        Task<int> GetCountAsync(Expression<Func<T, bool>>? expression);
    }

}

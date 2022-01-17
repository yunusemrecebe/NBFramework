using Core.Enums;
using Core.Extensions;
using Core.Utilities.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace NbFramework.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>, IDisposable
        where TEntity : class, IEntity, new()
        where TContext : DbContext
    {
        protected TContext Context { get; }
        protected string[] IncludeProperties { get; }
        private bool IsDisposed;

        public EfEntityRepositoryBase(TContext context)
        {
            Context = context;
        }

        public EfEntityRepositoryBase(TContext context, params string[] includeProperties)
        {
            Context = context;
            IncludeProperties = includeProperties;
        }

        public TEntity Add(TEntity entity)
        {
            return Context.Add(entity).Entity;
        }

        public TEntity Update(TEntity entity)
        {
            Context.Update(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            Context.Remove(entity);
        }

        public async Task<int> Execute(FormattableString interpolatedQueryString)
        {
            return await Context.Database.ExecuteSqlInterpolatedAsync(interpolatedQueryString);
        }

        #region [ Get ]

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>()
                    .AsQueryable()
                    .SingleOrDefault(expression);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>()
                    .AsQueryable()
                    .SingleOrDefaultAsync(expression);
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>()
                    .AsQueryable()
                    .FirstOrDefault(expression);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>()
                    .AsQueryable()
                    .FirstOrDefaultAsync(expression);
        }

        public int GetCount(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
                return Context.Set<TEntity>().Count();

            return Context.Set<TEntity>().Count(expression);
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
                return await Context.Set<TEntity>().CountAsync();

            return await Context.Set<TEntity>().CountAsync(expression);
        }

        #endregion

        #region [ Any ]

        public bool Any()
        {
            IQueryable<TEntity> baseQuery = GetBaseQuery();

            return baseQuery.Any();
        }

        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            IQueryable<TEntity> baseQuery = GetBaseQuery(expression);
            return baseQuery.Any();
        }

        #endregion

        #region [ GetList ]

        public IEnumerable<TEntity> GetList()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>()
                    .Where(expression)
                    .AsNoTracking();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>()
                    .Where(expression)
                    .ToListAsync();
        }

        public PagedListResult<TEntity> GetPagedList(int pageNumber, int pageSize, string orderBy, bool orderByDesc = true)
        {
            IQueryable<TEntity>? list = Context.Set<TEntity>().AsQueryable();

            if (IncludeProperties != null && IncludeProperties.Length > 0)
                list = list.IncludeMultiple(IncludeProperties);

            list = orderByDesc ? list.OrderByAscOrDesc(SortBy.DESC, orderBy) : list.OrderByAscOrDesc(SortBy.ASC, orderBy);

            int totalRecordCount = list.Count();
            int totalPageCount = totalRecordCount / pageSize;

            list = list.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedListResult<TEntity>(list.ToList(), pageNumber, pageSize, totalPageCount, totalRecordCount);
        }

        public PagedListResult<TEntity> GetPagedList(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize, string orderBy, bool orderByDesc = true)
        {
            IQueryable<TEntity>? list = Context.Set<TEntity>().AsQueryable();

            if (IncludeProperties != null && IncludeProperties.Length > 0)
                list = list.IncludeMultiple(IncludeProperties);

            list = list.Where(expression).AsQueryable();

            list = orderByDesc ? list.OrderByAscOrDesc(SortBy.DESC, orderBy) : list.OrderByAscOrDesc(SortBy.ASC, orderBy);

            int totalRecordCount = list.Count();
            int totalPageCount = totalRecordCount / pageSize;

            list = list.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedListResult<TEntity>(list.ToList(), pageNumber, pageSize, totalPageCount, totalRecordCount);
        }

        #endregion

        public Result InTransaction(Func<Result> transaction, Action? successAction, Action<Exception>? exceptionAction)
        {
            Result? result = default(Result);

            using (IDbContextTransaction? tx = Context.Database.BeginTransaction())
            {
                try
                {
                    result = transaction();
                    Commit();
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();

                    if (exceptionAction == null)
                    {
                        throw;
                    }

                    exceptionAction(ex);
                }

                successAction?.Invoke();

                return result;
            }
        }

        #region [ BaseQuery ]

        public IQueryable<TEntity> GetBaseQuery(bool noTracking = true)
        {
            IQueryable<TEntity> baseQuery = Context.Set<TEntity>();

            if (IncludeProperties != null && IncludeProperties.Length > 0)
                baseQuery = baseQuery.IncludeMultiple(IncludeProperties);

            if (noTracking)
                baseQuery = baseQuery.AsNoTracking();

            return baseQuery;
        }

        public IQueryable<TEntity> GetBaseQuery(Expression<Func<TEntity, bool>> expression, bool noTracking = true)
        {
            IQueryable<TEntity> baseQuery = Context.Set<TEntity>();

            if (IncludeProperties != null && IncludeProperties.Length > 0)
                baseQuery = baseQuery.IncludeMultiple(IncludeProperties);

            if (expression != null)
                baseQuery = baseQuery.Where(expression);

            if (noTracking)
                baseQuery = baseQuery.AsNoTracking();

            return baseQuery;
        }

        #endregion

        #region [ Commit ]

        public int Commit()
        {
            return Context.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return Context.SaveChangesAsync();
        }

        #endregion

        #region [ IDisposable ]

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}

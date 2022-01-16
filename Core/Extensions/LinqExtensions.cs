using Core.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params string[] includes) where T : class
        {
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

        public static IOrderedQueryable<TSource> OrderByAscOrDesc<TSource>(this IQueryable<TSource> query, SortBy sortBy, string propertyName)
        {
            Type? entityType = typeof(TSource);

            PropertyInfo? propertyInfo = entityType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (propertyInfo is null)
                propertyInfo = entityType.GetProperty("Id");

            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyInfo.Name);
            LambdaExpression? selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            Type? enumarableType = typeof(Queryable);

            string? sortType = sortBy == SortBy.ASC ? "OrderBy" : "OrderByDescending";

            MethodInfo? method = enumarableType.GetMethods()
                .Where(m => m.Name == sortType && m.IsGenericMethodDefinition)
                    .Single(m =>
                        {
                            List<ParameterInfo>? parameters = m.GetParameters().ToList();
                            return parameters.Count == 2;
                        });

            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            IOrderedQueryable<TSource>? newQuery = (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }
    }
}

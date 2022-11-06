using System.Linq.Expressions;
using System.Reflection;

namespace ProjectX.Persistence.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<TEntity> WithPagination<TEntity>(this IQueryable<TEntity> source, IPaginationOptions paginable)
        => paginable == default ? source : source.Skip(paginable.Skip).Take(paginable.Take);

    public static IQueryable<TEntity> WithOrdering<TEntity>(this IQueryable<TEntity> source, IOrderingOptions orderable)
        => source.OrderBy(orderable.OrderBy, orderable.Descending);

    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyName, bool reverse = false)
    {
        if (string.IsNullOrEmpty(propertyName) || typeof(TEntity).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public) == null)
            return source;

        return reverse ? source.OrderByDescending(propertyName) : source.OrderBy(propertyName);
    }

    public static IQueryable<TEntity> Where<TEntity, TValue>(this IQueryable<TEntity> source, string propertyName, TValue value) 
    {
        // t
        ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "t");
        // t.Name
        MemberExpression property = Expression.Property(parameter, propertyName);
        // value
        ConstantExpression constant = Expression.Constant(value);
        // t.Name == "value"
        BinaryExpression filter = Expression.Equal(property, constant);
        // t => t.Name == "value"
        var predicate = Expression.Lambda<Func<TEntity, bool>>(filter, parameter);

        return source.Where(predicate);
    }

    /// <summary>
    /// The value is parsed to the type of property
    ///  where(t => t.Id == int.parse("1"));
    ///  where(t => t.property == [type].parse(value))
    /// </summary>
    public static IQueryable<TEntity> DynamicWhere<TEntity>(this IQueryable<TEntity> source, string propertyName, string value)
    {
        // t
        ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "t");
        // t.Name
        MemberExpression property = Expression.Property(parameter, propertyName);
        // [type].Parse(value)
        PropertyInfo properyInfo = property.Member as PropertyInfo;
        MethodInfo parseMethod = properyInfo.PropertyType.GetMethod("Parse", new[] { typeof(string) });
        ConstantExpression constant = Expression.Constant(value);
        MethodCallExpression parseCall = Expression.Call(parseMethod, constant);

        // t.property == [type].parse(value)
        BinaryExpression filter = Expression.Equal(property, parseCall);
        // t => t.Name == "value"
        var predicate = Expression.Lambda<Func<TEntity, bool>>(filter, parameter);

        return source.Where(predicate);
    }

    private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        => source.OrderBy(CreateExpression<T>(propertyName));

    private static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        => source.OrderByDescending(CreateExpression<T>(propertyName));

    private static Expression<Func<T, object>> CreateExpression<T>(string propertyName)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T));
        MemberExpression property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}
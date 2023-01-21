using System.Linq.Expressions;
using System.Reflection;

namespace ProjectX.Core.Results;

public static class ResultActivator
{
    public static TResult From<TResult>(ApplicationError error)
    {
        var type = typeof(TResult);

        //To improve performance we can cache it to private static field
        ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public,
            null, new[] { typeof(ApplicationError) }, null);

        ParameterExpression errorParameter = Expression.Parameter(typeof(ApplicationError), "error");

        NewExpression constructorExpression = Expression.New(constructorInfo, errorParameter);

        Expression<Func<ApplicationError, object>> lambdaExpression =
            Expression.Lambda<Func<ApplicationError, object>>(constructorExpression, errorParameter);

        //To improve performance we can cache it to private static field
        var compiled = lambdaExpression.Compile();

        return (TResult)compiled(error);
    }
}
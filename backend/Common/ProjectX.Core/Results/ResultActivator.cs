using System.Linq.Expressions;
using System.Reflection;

namespace ProjectX.Core.Results;

public static class ResultActivator
{
    public static TResult From<TResult>(Error error)
    {
        var type = typeof(TResult);

        //To improve performance we can cache it to private static field
        ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public,
            null, new[] { typeof(Error) }, null);

        ParameterExpression errorParameter = Expression.Parameter(typeof(Error), "error");

        NewExpression constructorExpression = Expression.New(constructorInfo, errorParameter);

        Expression<Func<Error, object>> lambdaExpression =
            Expression.Lambda<Func<Error, object>>(constructorExpression, errorParameter);

        //To improve performance we can cache it to private static field
        var compiled = lambdaExpression.Compile();

        return (TResult)compiled(error);
    }
}
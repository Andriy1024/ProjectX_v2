using LinqSpecs;
using System.Linq.Expressions;

namespace ProjectX.Core;

public class EmptySpecification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
        => e => true;
}
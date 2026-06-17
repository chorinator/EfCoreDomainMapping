using System.Linq.Expressions;

namespace EfCoreMapping.Domain.Specifications;

public abstract class Specification<T>(Expression<Func<T, bool>> criteria)
{
    public Expression<Func<T, bool>> Criteria => criteria;
}

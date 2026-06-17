using EfCoreMapping.Domain.Specifications;

namespace EfCoreMapping.Infrastructure.EfCore;

public static class QueryableExtensions
{
    public static IQueryable<T> WithSpecification<T>(this IQueryable<T> query, Specification<T> spec)
        => query.Where(spec.Criteria);
}

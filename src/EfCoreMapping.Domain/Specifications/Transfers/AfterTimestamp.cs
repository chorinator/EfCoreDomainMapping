namespace EfCoreMapping.Domain.Specifications.Transfers;

public sealed class AfterTimestamp(Timestamp from)
    : Specification<Transfer>(t => t.ExecutedAt >= from);

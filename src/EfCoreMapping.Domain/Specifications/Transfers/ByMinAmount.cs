namespace EfCoreMapping.Domain.Specifications.Transfers;

public sealed class ByMinAmount(decimal min)
    : Specification<Transfer>(t => t.Amount.Amount >= min);

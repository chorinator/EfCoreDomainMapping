namespace EfCoreMapping.Domain.Specifications.Transfers;

public sealed class ByCurrency(Currency currency)
    : Specification<Transfer>(t => t.Amount.Currency == currency);

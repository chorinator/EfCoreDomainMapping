namespace EfCoreMapping.Domain.Specifications.Transfers;

public sealed class ByCurrencyStartingWith(string prefix)
    : Specification<Transfer>(t => t.Amount.Currency.Code.StartsWith(prefix));

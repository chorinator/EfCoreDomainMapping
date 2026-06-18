namespace EfCoreMapping.Domain.Queries.Transfers;

public static class TransferQueries
{
    extension(IQueryable<Transfer> queryable)
    {
        public IQueryable<Transfer> ByCurrency(Currency currency)
            => queryable.Where(t => t.Amount.Currency == currency);

        public IQueryable<Transfer> ByCurrencyStartingWith(string prefix)
            => queryable.Where(t => t.Amount.Currency.Code.StartsWith(prefix));

        public IQueryable<Transfer> ByMinAmount(decimal minimumAmount)
            => queryable.Where(t => t.Amount.Amount >= minimumAmount);

        public IQueryable<Transfer> AfterTimestamp(Timestamp from)
            => queryable.Where(t => t.ExecutedAt >= from);
    }
}

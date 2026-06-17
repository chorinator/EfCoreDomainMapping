using EfCoreMapping.Domain;

namespace EfCoreMapping.Infrastructure.EfCore.Specifications;

public static class QueryTransfersDemo
{
    public static IEnumerable<Transfer> QueryByCurrency(AppDbContext db, Currency currency)
    {
        var transfers = db.Transfers
            .Where(transfer => transfer.Amount.Currency == currency)
            .ToList();

        return transfers;
    }

    public static IEnumerable<Transfer> QueryByCurrencyStartingWith(AppDbContext db, string currencyCodePrefix)
    {
        var transfers = db.Transfers
            .Where(transfer => transfer.Amount.Currency.Code.StartsWith(currencyCodePrefix))
            .ToList();
        
        return transfers;
    }

    public static IEnumerable<Transfer> QueryByAmount(AppDbContext db, decimal minAmount)
    {
        var transfers = db.Transfers
            .Where(transfer => transfer.Amount.Amount >= minAmount)
            .ToList();
        
        return transfers;
    }

    public static IEnumerable<Transfer> QueryByTime(AppDbContext db, Timestamp from)
    {
        var transfers = db.Transfers
            .Where(transfer => transfer.ExecutedAt >= from)
            .ToList();
        
        return transfers;
    }
}
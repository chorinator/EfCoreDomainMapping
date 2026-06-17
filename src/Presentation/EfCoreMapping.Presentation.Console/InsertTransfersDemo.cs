using EfCoreMapping.Domain;
using EfCoreMapping.Infrastructure.EfCore;

namespace EfCoreMapping.Presentation.Console;

public static class InsertTransfersDemo
{
    public static void InsertTransfers(AppDbContext context)
    {
        var usd = Currency.USD;
        var jpy = Currency.JPY;
        var now = Timestamp.UtcNow;
        var lag = TimeSpan.FromHours(2);

        decimal[] usdAmounts = [10m, 2.4m, 200m, 35.11m];
        decimal[] jpyAmounts = [340m, 2100m];

        var amounts =
            usdAmounts.Select(am => new Money(am, usd))
                .Concat(jpyAmounts.Select(am => new Money(am, jpy)));

        foreach (var amount in amounts)
        {
            var transfer = new Transfer(TransferId.NewId(), amount, now);
            context.Transfers.Add(transfer);
            now = now.Add(lag);
        }
        
        context.SaveChanges();

        System.Console.WriteLine();
        System.Console.WriteLine($"Inserted {usdAmounts.Length} USD and {jpyAmounts.Length} JPY transfers..");
        System.Console.WriteLine();
    }
}
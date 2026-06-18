using EfCoreMapping.Domain;
using EfCoreMapping.Domain.Queries.Transfers;
using EfCoreMapping.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;

namespace EfCoreMapping.Presentation.Console;

public class App(AppDbContext db)
{
    public Task RunAsync()
    {
        if (db.Database.GetPendingMigrations().Any())
            db.Database.Migrate();

        if (!db.Transfers.Any())
            InsertTransfersDemo.InsertTransfers(db);

        var timestamp = Timestamp.UtcNow.Add(TimeSpan.FromHours(3));
        List<(string Title, Func<IQueryable<Transfer>, IQueryable<Transfer>> Apply)> queries =
        [
            ("USD transfers",              q => q.ByCurrency(Currency.USD)),
            ("Currency starting with 'J'", q => q.ByCurrencyStartingWith("J")),
            ("Amount >= 100",              q => q.ByMinAmount(100m)),
            ($"Since {timestamp}",         q => q.AfterTimestamp(timestamp))
        ];

        foreach (var (title, apply) in queries)
        {
            System.Console.WriteLine($"--- {title} ---");
            foreach (var t in apply(db.Transfers).ToList())
                System.Console.WriteLine($"  {t}");
        }

        return Task.CompletedTask;
    }
}

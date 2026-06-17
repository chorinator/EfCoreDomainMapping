using EfCoreMapping.Domain;
using EfCoreMapping.Domain.Specifications;
using EfCoreMapping.Domain.Specifications.Transfers;
using EfCoreMapping.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;

namespace EfCoreMapping.Presentation.Console;

public class App(AppDbContext db)
{
    public Task RunAsync()
    {
        if (db.Database.GetPendingMigrations().Any())
            db.Database.Migrate();

        if(!db.Transfers.Any())
            InsertTransfersDemo.InsertTransfers(db);

        var timestamp = Timestamp.UtcNow.Add(TimeSpan.FromHours(3));
        List<Tuple<string, Specification<Transfer>>> specifications =
        [
            new ("USD transfers", new ByCurrency(Currency.USD)),
            new ("Currency Starting With 'J'", new ByCurrencyStartingWith("J")),
            new ("Transfers with amount >= 100", new ByMinAmount(100m)),
            new ($"Transfers executed since {timestamp}", new AfterTimestamp(timestamp))
        ];

        foreach (var (title, specification) in specifications)
        {
            System.Console.WriteLine($"--- {title} ---");
            var results = 
                db.Transfers.WithSpecification(specification).ToList();
            foreach (var transfer in results)
                System.Console.WriteLine($"  {transfer}");
        }
        
        return Task.CompletedTask;
    }
}

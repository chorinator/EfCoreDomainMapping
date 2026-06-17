using EfCoreMapping.Domain;
using EfCoreMapping.Infrastructure.EfCore;
using EfCoreMapping.Infrastructure.EfCore.Specifications;

namespace EfCoreMapping.Presentation.Console;

public class App(AppDbContext db)
{
    public Task RunAsync()
    {
        InsertTransfersDemo.InsertTransfers(db);

        System.Console.WriteLine("--- USD transfers ---");
        foreach (var t in QueryTransfersDemo.QueryByCurrency(db, Currency.USD))
            System.Console.WriteLine($"  {t}");

        System.Console.WriteLine("--- JPY transfers ---");
        foreach (var t in QueryTransfersDemo.QueryByCurrency(db, Currency.JPY))
            System.Console.WriteLine($"  {t}");

        System.Console.WriteLine("--- Transfers >= 50 ---");
        foreach (var t in QueryTransfersDemo.QueryByAmount(db, 50m))
            System.Console.WriteLine($"  {t}");

        return Task.CompletedTask;
    }
}

using System.Diagnostics;

namespace EfCoreMapping.Infrastructure.EfCore.Tests;

public static class Helper
{
    public static CancellationTokenSource GetCancellationTokenSource()
    {
        return Debugger.IsAttached
            ? new CancellationTokenSource(TimeSpan.FromHours(1))
            : new CancellationTokenSource(TimeSpan.FromSeconds(10));
    }
}
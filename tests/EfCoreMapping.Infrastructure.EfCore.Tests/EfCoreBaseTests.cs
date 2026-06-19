using System.Diagnostics;

namespace EfCoreMapping.Infrastructure.EfCore.Tests;

public abstract class EfCoreBaseTests : IAsyncLifetime
{
    protected abstract Task<AppDbContext> CreateDbContext(CancellationToken ct);

    private readonly List<CancellationTokenSource> _dispatchedCancellationTokenSources = [];
    protected CancellationToken GetCancellationToken()
    {
        var cts = Debugger.IsAttached
            ? new CancellationTokenSource(TimeSpan.FromHours(1))
            : new CancellationTokenSource(TimeSpan.FromSeconds(10));
        _dispatchedCancellationTokenSources.Add(cts);
        
        return cts.Token;
    }
    
    protected AppDbContext? DbContext;

    public virtual async ValueTask DisposeAsync()
    {
        foreach (var cts in _dispatchedCancellationTokenSources)
        {
            await cts.CancelAsync();
            cts.Dispose();
        }
        
        if(DbContext is not null)
            await DbContext.DisposeAsync();
    }

    public virtual async ValueTask InitializeAsync()
    {
        DbContext = await CreateDbContext(GetCancellationToken());
    }
}
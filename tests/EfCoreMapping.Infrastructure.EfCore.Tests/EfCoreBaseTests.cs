namespace EfCoreMapping.Infrastructure.EfCore.Tests;

public abstract class EfCoreBaseTests : IAsyncLifetime
{
    private readonly List<CancellationTokenSource> _dispatchedCancellationTokenSources = [];
    protected CancellationToken GetCancellationToken()
    {
        var cts = Helper.GetCancellationTokenSource();
        _dispatchedCancellationTokenSources.Add(cts);
        
        return cts.Token;
    }

    #region DbContext
    
    protected abstract Task ResetState(CancellationToken ct);
    protected abstract AppDbContext GetDbContext();
    
    #endregion
    public virtual async ValueTask DisposeAsync()
    {
        foreach (var cts in _dispatchedCancellationTokenSources)
        {
            await cts.CancelAsync();
            cts.Dispose();
        }
        
        GC.SuppressFinalize(this);
    }

    public virtual async ValueTask InitializeAsync()
    {
        await ResetState(GetCancellationToken());
    }
}
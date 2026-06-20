using EfCoreMapping.Infrastructure.EfCore.Tests;
using Testcontainers.MsSql;

namespace EfCoreMapping.Infrastructure.MsSql.Tests;

public sealed class MsSqlDatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container =
        new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
            .WithName($"mssql-{Guid.NewGuid():N}")
            .Build();

    private readonly CancellationTokenSource _cts = Helper.GetCancellationTokenSource();

    public MsSqlContainer GetContainer() => _container;

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync(_cts.Token);
    }

    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
        _cts.Dispose();
    }
}

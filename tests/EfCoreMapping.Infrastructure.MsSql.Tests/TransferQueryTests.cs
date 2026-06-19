using EfCoreMapping.Infrastructure.EfCore;
using EfCoreMapping.Infrastructure.EfCore.Tests;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace EfCoreMapping.Infrastructure.MsSql.Tests;

public class TransferQueryTests : TransferQueriesTestBase
{
    private MsSqlContainer? _container;

    protected override async Task<AppDbContext> CreateDbContext(CancellationToken ct)
    {
        _container = 
            new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
            .Build();

        await _container.StartAsync(ct);

        var connectionString = _container.GetConnectionString();
        var options = new DbContextOptionsBuilder<MsSqlAppDbContext>()
            .UseSqlServer(connectionString,
                mssqlOptions =>
                    mssqlOptions.MigrationsAssembly(
                        typeof(MsSqlAppDbContext).Assembly.GetName().Name))
            .Options;
        
        DbContext = new MsSqlAppDbContext(options);
        
        await DbContext.Database.MigrateAsync(ct);
        
        return DbContext;
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        
        if(_container is not null)
            await _container.DisposeAsync();
    }
}
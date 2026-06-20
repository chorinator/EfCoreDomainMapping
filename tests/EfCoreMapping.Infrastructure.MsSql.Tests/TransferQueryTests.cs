using EfCoreMapping.Infrastructure.EfCore;
using EfCoreMapping.Infrastructure.EfCore.Tests;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace EfCoreMapping.Infrastructure.MsSql.Tests;

public class TransferQueryTests(MsSqlDatabaseFixture fixture)
    : TransferQueriesTestBase, IClassFixture<MsSqlDatabaseFixture>
{
    private MsSqlAppDbContext _dbContext = null!;

    public override async ValueTask InitializeAsync()
    {
        _dbContext = CreateDbContext(fixture.GetContainer());
        await _dbContext.Database.MigrateAsync();
        await base.InitializeAsync();
    }

    protected override AppDbContext GetDbContext() => _dbContext;

    protected override Task ResetState(CancellationToken ct)
        => _dbContext.Transfers.ExecuteDeleteAsync(ct);

    private static MsSqlAppDbContext CreateDbContext(MsSqlContainer container)
    {
        var options = new DbContextOptionsBuilder<MsSqlAppDbContext>()
            .UseSqlServer(container.GetConnectionString(),
                o => o.MigrationsAssembly(typeof(MsSqlAppDbContext).Assembly.GetName().Name))
            .Options;

        return new MsSqlAppDbContext(options);
    }
    
    public override async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await base.DisposeAsync();
    }
}

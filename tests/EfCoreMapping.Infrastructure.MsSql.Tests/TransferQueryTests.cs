using EfCoreMapping.Infrastructure.EfCore;
using EfCoreMapping.Infrastructure.EfCore.Tests;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace EfCoreMapping.Infrastructure.MsSql.Tests;

public class TransferQueryTests(MsSqlDatabaseFixture fixture)
    : TransferQueriesTestBase, IClassFixture<MsSqlDatabaseFixture>
{
    private MsSqlContainer? _container;
    private MsSqlAppDbContext? _dbContext;
    
    public override async ValueTask InitializeAsync()
    {
        _container = await fixture.GetContainer();
        
        await base.InitializeAsync();
    }

    protected override AppDbContext GetDbContext()
        => CreateDbContext(_container!);

    protected override Task ResetState(CancellationToken ct)
        => GetDbContext().Transfers.ExecuteDeleteAsync(ct);
    
    private MsSqlAppDbContext CreateDbContext(MsSqlContainer container)
    {
        if(_dbContext is not null)
            return _dbContext;
        
        var options = new DbContextOptionsBuilder<MsSqlAppDbContext>()
            .UseSqlServer(container.GetConnectionString(),
                o => o.MigrationsAssembly(typeof(MsSqlAppDbContext).Assembly.GetName().Name))
            .Options;

        _dbContext = new MsSqlAppDbContext(options);
        _dbContext.Database.Migrate();

        return _dbContext;
    }
}

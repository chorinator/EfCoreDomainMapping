using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EfCoreMapping.Infrastructure.Postgres;

public class PostgresAppDbContextFactory : IDesignTimeDbContextFactory<PostgresAppDbContext>
{
    public PostgresAppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresAppDbContext>();
        optionsBuilder.UseNpgsql(string.Empty);
        
        return new PostgresAppDbContext(optionsBuilder.Options);
    }
}
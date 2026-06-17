using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EfCoreMapping.Infrastructure.MsSql;

public class MsSqlAppDbContextFactory : IDesignTimeDbContextFactory<MsSqlAppDbContext>
{
    public MsSqlAppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MsSqlAppDbContext>();
        optionsBuilder.UseSqlServer(string.Empty);
        
        return new MsSqlAppDbContext(optionsBuilder.Options);
    }
}
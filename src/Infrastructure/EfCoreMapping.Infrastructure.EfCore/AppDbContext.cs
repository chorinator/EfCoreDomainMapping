using EfCoreMapping.Domain;
using Microsoft.EntityFrameworkCore;

namespace EfCoreMapping.Infrastructure.EfCore;

public abstract class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Transfer> Transfers => Set<Transfer>();
}

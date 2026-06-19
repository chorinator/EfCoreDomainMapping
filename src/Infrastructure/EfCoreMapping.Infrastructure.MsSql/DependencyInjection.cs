using EfCoreMapping.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EfCoreMapping.Infrastructure.MsSql;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlServerSupport(this IServiceCollection services, string connectionString)
    {
        if(string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string is empty");
        
        services.AddDbContext<AppDbContext, MsSqlAppDbContext>(options =>
        {
            options.UseSqlServer(connectionString,
                mssqlOptions =>
                    mssqlOptions.MigrationsAssembly(
                        typeof(MsSqlAppDbContext).Assembly.GetName().Name));
        });
        
        return services;
    }
}
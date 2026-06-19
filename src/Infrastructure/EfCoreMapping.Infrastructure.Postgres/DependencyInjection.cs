using EfCoreMapping.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EfCoreMapping.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresSupport(this IServiceCollection services, string connectionString)
    {
        if(string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string is empty");
        
        services.AddDbContext<AppDbContext, PostgresAppDbContext>(options =>
        {
            options.UseNpgsql(connectionString,
                npgsqlOptions =>
                    npgsqlOptions.MigrationsAssembly(
                        typeof(PostgresAppDbContext).Assembly.GetName().Name));
        });
        
        return services;
    }
}
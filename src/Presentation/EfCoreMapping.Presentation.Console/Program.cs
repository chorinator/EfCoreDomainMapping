using EfCoreMapping.Infrastructure.EfCore;
using EfCoreMapping.Infrastructure.Postgres;
using EfCoreMapping.Presentation.Console;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var connectionString = context.Configuration.GetConnectionString("Default")!;
        if(string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string is empty");
        
        services.AddDbContext<AppDbContext, PostgresAppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, 
                npgsqlOptions => 
                    npgsqlOptions.MigrationsAssembly(
                        typeof(PostgresAppDbContext).Assembly.GetName().Name));
        });
        services.AddTransient<App>();
    })
    .Build();

using var scope = host.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<App>().RunAsync();

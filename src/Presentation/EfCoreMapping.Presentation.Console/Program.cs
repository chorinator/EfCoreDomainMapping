using EfCoreMapping.Infrastructure.MsSql;
using EfCoreMapping.Infrastructure.Postgres;
using EfCoreMapping.Presentation.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var dbProviderSelector = context.Configuration.GetConnectionString("Selector")!;
        if(string.IsNullOrWhiteSpace(dbProviderSelector))
            throw new InvalidOperationException("Database provider not recognized. Use SqlServer or Postgres");
        
        var connectionString = context.Configuration.GetConnectionString(dbProviderSelector)!;
        if(string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string is empty");
        
        // Postgres
        if (string.Equals(dbProviderSelector, "Postgres", StringComparison.OrdinalIgnoreCase))
            services.AddPostgresSupport(connectionString);
        
        // MsSql
        if (string.Equals(dbProviderSelector, "SqlServer", StringComparison.OrdinalIgnoreCase))
            services.AddSqlServerSupport(connectionString);
        
        services.AddTransient<App>();
    })
    .Build();

using var scope = host.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<App>().RunAsync();

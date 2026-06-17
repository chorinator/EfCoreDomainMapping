using EfCoreMapping.Domain;
using EfCoreMapping.Infrastructure.EfCore;
using EfCoreMapping.Infrastructure.Postgres.Conversions;
using Microsoft.EntityFrameworkCore;

namespace EfCoreMapping.Infrastructure.Postgres;

public class PostgresAppDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transfer>(transferBuilder =>
        {
            transferBuilder.Property<int>("Key")
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();
            transferBuilder.HasKey("Key");

            transferBuilder.Property(transfer => transfer.Id)
                .HasConversion(new TransferIdConverter())
                .HasColumnName("PublicId")
                .HasColumnType("uniqueidentifier");
            transferBuilder.HasIndex(transfer => transfer.Id).IsUnique();
            
            transferBuilder.Property(transfer => transfer.ExecutedAt)
                .HasConversion(new TimestampConverter())
                .HasColumnType("datetime2");

            transferBuilder.ComplexProperty(t => t.Amount, moneyBuilder =>
            {
                moneyBuilder.Property(money => money.Amount)
                    .HasColumnName("Amount")
                    .HasColumnType("numeric(18, 3)");

                moneyBuilder.ComplexProperty(money => money.Currency, currencyBuilder =>
                {
                    currencyBuilder.Property(currency => currency.Code)
                        .HasColumnName("CurrencyCode")
                        .HasColumnType("varchar(3)");
                    currencyBuilder.Property(currency => currency.DecimalPlaces)
                        .HasColumnName("CurrencyDecimalPlaces");
                });
            });
        });
    }
}

using EfCoreMapping.Domain;
using EfCoreMapping.Domain.Queries.Transfers;
using Microsoft.EntityFrameworkCore;

namespace EfCoreMapping.Infrastructure.EfCore.Tests;

public abstract class TransferQueriesTestBase : EfCoreBaseTests
{
    private static readonly Timestamp BaseTime = new(new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc));

    private static Transfer MakeTransfer(Currency currency, decimal amount, Timestamp? executedAt = null)
        => new(TransferId.NewId(), new Money(amount, currency), executedAt ?? BaseTime);

    private AppDbContext DbContext => GetDbContext();

    private async Task Seed(params Transfer[] transfers)
    {
        await DbContext.Transfers.AddRangeAsync(transfers);
        await DbContext.SaveChangesAsync();
    }

    // ByCurrency

    [Fact]
    public async Task ByCurrency_ReturnsOnlyMatchingCurrency()
    {
        // Arrange
        var usd = MakeTransfer(Currency.USD, 100m);
        var eur = MakeTransfer(Currency.EUR, 200m);
        await Seed(usd, eur);

        // Act
        var result =
            await DbContext.Transfers
                .ByCurrency(Currency.USD)
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Single(result);
        Assert.Equal(usd.Id, result[0].Id);
    }

    [Fact]
    public async Task ByCurrency_NoMatch_ReturnsEmpty()
    {
        // Arrange
        await Seed(MakeTransfer(Currency.EUR, 50m));

        // Act
        var result =
            await DbContext.Transfers
                .ByCurrency(Currency.USD)
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ByCurrency_MultipleTransfersSameCurrency_ReturnsAll()
    {
        // Arrange
        await Seed(MakeTransfer(Currency.USD, 10m), MakeTransfer(Currency.USD, 20m));

        // Act
        var result =
            await DbContext.Transfers
                .ByCurrency(Currency.USD)
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Equal(2, result.Count);
    }

    // ByCurrencyStartingWith

    [Fact]
    public async Task ByCurrencyStartingWith_ReturnsMatchingPrefix()
    {
        // Arrange
        var usd = MakeTransfer(Currency.USD, 100m);
        var jpy = MakeTransfer(Currency.JPY, 300m);
        await Seed(usd, jpy);

        // Act
        var result =
            await DbContext.Transfers
                .ByCurrencyStartingWith("J")
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Single(result);
        Assert.Equal(jpy.Id, result[0].Id);
    }

    [Fact]
    public async Task ByCurrencyStartingWith_NoMatch_ReturnsEmpty()
    {
        // Arrange
        await Seed(MakeTransfer(Currency.USD, 100m));

        // Act
        var result =
            await DbContext.Transfers
                .ByCurrencyStartingWith("X")
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ByCurrencyStartingWith_MultipleMatches_ReturnsAll()
    {
        // Arrange
        var xa1 = MakeTransfer(new Currency("XA1", 2), 10m);
        var xa2 = MakeTransfer(new Currency("XA2", 2), 20m);
        var eur = MakeTransfer(Currency.EUR, 30m);
        await Seed(xa1, xa2, eur);

        // Act
        var result =
            await DbContext.Transfers
                .ByCurrencyStartingWith("XA")
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Equal(2, result.Count);
    }

    // ByMinAmount

    [Fact]
    public async Task ByMinAmount_ReturnsTransfersAtOrAboveMinimum()
    {
        // Arrange
        var low = MakeTransfer(Currency.USD, 50m);
        var exact = MakeTransfer(Currency.USD, 100m);
        var high = MakeTransfer(Currency.USD, 200m);
        await Seed(low, exact, high);

        // Act
        var result =
            await DbContext.Transfers
                .ByMinAmount(100m)
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Equal(2, result.Count);
        Assert.DoesNotContain(result, t => t.Id == low.Id);
    }

    [Fact]
    public async Task ByMinAmount_ExactBoundary_IsIncluded()
    {
        // Arrange
        var transfer = MakeTransfer(Currency.USD, 100m);
        await Seed(transfer);

        // Act
        var result =
            await DbContext.Transfers
                .ByMinAmount(100m)
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task ByMinAmount_AllBelowMinimum_ReturnsEmpty()
    {
        // Arrange
        await Seed(MakeTransfer(Currency.USD, 10m), MakeTransfer(Currency.EUR, 50m));

        // Act
        var result =
            await DbContext.Transfers
                .ByMinAmount(100m)
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Empty(result);
    }

    // AfterTimestamp

    [Fact]
    public async Task AfterTimestamp_ReturnsTransfersAtOrAfterGivenTime()
    {
        // Arrange
        var before = MakeTransfer(Currency.USD, 100m, BaseTime.Add(TimeSpan.FromHours(-1)));
        var exact = MakeTransfer(Currency.USD, 100m, BaseTime);
        var after = MakeTransfer(Currency.USD, 100m, BaseTime.Add(TimeSpan.FromHours(1)));
        await Seed(before, exact, after);

        // Act
        var result =
            await DbContext.Transfers
                .AfterTimestamp(BaseTime)
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Equal(2, result.Count);
        Assert.DoesNotContain(result, t => t.Id == before.Id);
    }

    [Fact]
    public async Task AfterTimestamp_ExactBoundary_IsIncluded()
    {
        // Arrange
        var transfer = MakeTransfer(Currency.USD, 100m, BaseTime);
        await Seed(transfer);

        // Act
        var result =
            await DbContext.Transfers
                .AfterTimestamp(BaseTime)
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task AfterTimestamp_AllBefore_ReturnsEmpty()
    {
        // Arrange
        var pastTime = BaseTime.Add(TimeSpan.FromHours(-2));
        await Seed(MakeTransfer(Currency.USD, 100m, pastTime));

        // Act
        var result =
            await DbContext.Transfers
                .AfterTimestamp(BaseTime)
                .ToListAsync(cancellationToken: GetCancellationToken());

        // Assert
        Assert.Empty(result);
    }
}

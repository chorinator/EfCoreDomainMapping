namespace EfCoreMapping.Domain;

public record Money(decimal Amount, Currency Currency)
{
    private Money() : this(default, default!) { }   // Required by EF Core
    
    public decimal Amount { get; init; } 
        = Math.Round(Amount, Currency?.DecimalPlaces ?? 0);

    public override string ToString() =>
        $"{Amount} {Currency}";
}
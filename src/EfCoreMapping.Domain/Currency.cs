namespace EfCoreMapping.Domain;

public record Currency(string Code, int DecimalPlaces)
{
    public string Code { get; init; } =
        !string.IsNullOrWhiteSpace(Code)
            ? Code
            : throw new ArgumentNullException($"{nameof(Code)} cannot be null or whitespace");
    
    public static readonly Currency USD = new("USD", 2);
    public static readonly Currency EUR = new("EUR", 2);
    public static readonly Currency GBP = new("GBP", 2);
    public static readonly Currency JPY = new("JPY", 0);
    
    public override string ToString() => Code;
}
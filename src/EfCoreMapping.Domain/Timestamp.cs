namespace EfCoreMapping.Domain;

public record Timestamp(DateTime Value)
{
    public DateTime Value { get; init; } =
        Value.Kind == DateTimeKind.Utc
            ? Value
            : throw new ArgumentException("Timestamp must be UTC time.", nameof(Value));

    public static Timestamp UtcNow => 
        new (DateTime.UtcNow);

    public Timestamp Add(TimeSpan timeSpan) =>
        new (Value.Add(timeSpan));
    
    public static bool operator >(Timestamp left, Timestamp right) => left.Value > right.Value;
    public static bool operator >=(Timestamp left, Timestamp right) => left.Value >= right.Value;
    public static bool operator <(Timestamp left, Timestamp right) => left.Value < right.Value;
    public static bool operator <=(Timestamp left, Timestamp right) => left.Value <= right.Value;
}
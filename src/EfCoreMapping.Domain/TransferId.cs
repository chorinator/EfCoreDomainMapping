namespace EfCoreMapping.Domain;

public record TransferId(Guid Id)
{
    public Guid Id { get; init; } = 
        Id != Guid.Empty ? Id
            : throw new ArgumentException("TransferId cannot be empty.", nameof(Id));
    
    public static TransferId NewId() => new(Guid.NewGuid());

    public override string ToString() =>
        Id.ToString()[..3] + "..." + Id.ToString()[^3..];
}
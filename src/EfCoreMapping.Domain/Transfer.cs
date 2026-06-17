namespace EfCoreMapping.Domain;

public class Transfer(TransferId id, Money amount, Timestamp executedAt)
{
    private Transfer() : this(default!, default!, default!)
    { } // Required by EF Core

    public TransferId Id { get; init; }
    public Money Amount { get; init; }
    public Timestamp ExecutedAt { get; init; }

    public override string ToString() =>
        $"Transfer {Id}: {Amount,15} at {ExecutedAt.Value:HH:mm}";
}
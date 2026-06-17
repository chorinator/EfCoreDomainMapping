namespace EfCoreMapping.Domain;

public class Transfer(TransferId id, Money amount, Timestamp executedAt)
{
    private Transfer() : this(default!, default!, default!)
    { } // Required by EF Core

    public TransferId Id { get; init; } = id;
    public Money Amount { get; init; } = amount;
    public Timestamp ExecutedAt { get; init; }  = executedAt;

    public override string ToString() =>
        $"Transfer {Id}: {Amount,15} at {ExecutedAt.Value:HH:mm}";
}
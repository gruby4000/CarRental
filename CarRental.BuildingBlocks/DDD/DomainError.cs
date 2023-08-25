namespace CarRental.BuildingBlocks.DDD;
public sealed record DomainError {
    public required string Message {get; init; }
    public required string Source {get; init; }
}
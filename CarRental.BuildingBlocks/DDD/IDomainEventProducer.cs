namespace CarRental.BuildingBlocks.DDD;

public interface IDomainEventProducer
{
    HashSet<DomainEvent> Events { get; }

}

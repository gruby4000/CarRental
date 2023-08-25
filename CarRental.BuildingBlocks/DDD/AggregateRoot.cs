using CarRental.BuildingBlocks.ErrorNotification;
using CarRental.BuildingBlocks.Validation;

namespace CarRental.BuildingBlocks.DDD;

public abstract class AggregateRoot : Entity, IDomainEventProducer
{
    public HashSet<DomainEvent> Events { get; private set; } = new();

}
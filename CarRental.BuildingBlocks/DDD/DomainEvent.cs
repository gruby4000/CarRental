using MediatR;

namespace CarRental.BuildingBlocks.DDD;

public abstract record DomainEvent: INotification {
    public Guid UniqueId {get; private set;}

    public DomainEvent()
    {
        UniqueId = new Guid();
    }

    public DomainEvent(Guid uniqueId)
    {
        UniqueId = uniqueId;
    }
}
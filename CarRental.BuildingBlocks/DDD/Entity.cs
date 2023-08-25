using System.ComponentModel.DataAnnotations.Schema;
using CarRental.BuildingBlocks.ErrorNotification;
using CarRental.BuildingBlocks.Validation;

namespace CarRental.BuildingBlocks.DDD;

public abstract class Entity: INotificationProducer<DomainError>, IValidatable, IIdentifable
{
    public int Id { get; init; } = 0;
    public Guid FrontId { get; init; } = Guid.NewGuid();
    [NotMapped]
    public Notification<DomainError> Notification { get; private set; }
    public abstract bool Validate();

    public Entity()
    {
        Notification = new();
    }
}

using CarRental.BuildingBlocks.ErrorNotification;

namespace CarRental.BuildingBlocks.DDD;

public class DomainException : Exception
{
    public Notification<DomainError> Notification {get; private set;}
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Notification<DomainError> notification) : base(message)
    {
        Notification = notification;
    }
    public DomainException(Notification<DomainError> notification): base()
    {
        Notification = notification;
    }
}
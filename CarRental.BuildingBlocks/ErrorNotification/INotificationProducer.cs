namespace CarRental.BuildingBlocks.ErrorNotification;

public interface INotificationProducer<T> where T: class
{
    protected Notification<T> Notification {get;}
}

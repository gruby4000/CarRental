namespace CarRental.BuildingBlocks.ServiceIntegration;

public interface IEventBus: IDisposable
{
    void PublishAll(params IEvent[] events);
    void Publish(IEvent @event);
    void Subscribe(IEventConsumer<IEvent> eventName);
    void Unsubscribe(string eventName);
}
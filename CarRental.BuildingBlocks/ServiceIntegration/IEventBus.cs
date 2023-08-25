namespace CarRental.BuildingBlocks.ServiceIntegration;

public interface IEventBus
{
    Task PublishAllAsync(string routingKey = null!);
    Task PublishSpecificEventAsync(IEvent @event, string routingKey = null!);
    void AddEventToPublish(IEvent @event);
    void SubscribeEvent(IEvent @event);
    void UnsubscribeEvent(IEvent @event);
    
}
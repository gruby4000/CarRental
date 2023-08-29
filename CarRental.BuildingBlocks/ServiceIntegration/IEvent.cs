namespace CarRental.BuildingBlocks.ServiceIntegration;

public interface IEvent
{
    Guid CorrelationId { get; }
    public DateTime Timestamp { get; }
    public string EventName { get; }
};
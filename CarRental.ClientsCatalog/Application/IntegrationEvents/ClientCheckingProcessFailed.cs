using CarRental.BuildingBlocks.ServiceIntegration;

namespace CarRental.ClientsCatalog.Application.IntegrationEvents;

public record ClientCheckingProcessFailed : IEvent
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public DateTime Timestamp { get; init; } = DateTime.Now;
    public string EventName => nameof(ClientCheckingProcessFailed);
    
}
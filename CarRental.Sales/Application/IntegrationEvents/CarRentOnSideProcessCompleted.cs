using CarRental.BuildingBlocks.ServiceIntegration;
using CarRental.HR.Model;

namespace CarRental.Sales.Application.IntegrationEvents;

public record CarRentOnSideProcessCompleted : IEvent
{
    public required string CarNumber { get; init; }
    public required Address NewCarAddress { get; init; }
    public required string RentNumber { get; init; }
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public DateTime Timestamp { get; } = DateTime.Now;
    public string EventName => nameof(CarRentOnSideProcessStarted);
}
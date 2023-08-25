using MassTransit.Futures.Contracts;

namespace CarRental.BuildingBlocks.ServiceIntegration.RabbitMQ;

public record RabbitMQOptions
{
    public required string HostName { get; init; } 
    public required string QueueName { get; init; }
    public bool AutoDelete { get; init; }
    public bool Exclusive { get; init; }
    public bool Durable { get; init; }
    public string? RoutingKey { get; init; }
    public required string ServiceId { get; init; }
    public string Exchange { get; init; } = null!;
    public List<string> Arguments { get; init; } = null!;

}
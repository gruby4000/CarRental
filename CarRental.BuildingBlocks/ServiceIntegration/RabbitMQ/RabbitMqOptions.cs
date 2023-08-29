using MassTransit.Futures.Contracts;

namespace CarRental.BuildingBlocks.ServiceIntegration.RabbitMQ;

public record RabbitMqOptions
{
    public required string HostName { get; init; } 
    public required string ConsumerQueueName { get; init; }
    public required int Ttl { get; init; }
    public bool AutoDelete { get; init; }
    public bool Exclusive { get; init; }
    public bool Durable { get; init; }
    public required string ServiceId { get; init; }
    public List<string> Arguments { get; init; } = null!;

}
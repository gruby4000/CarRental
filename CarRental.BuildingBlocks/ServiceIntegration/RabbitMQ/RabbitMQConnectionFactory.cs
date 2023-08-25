using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CarRental.BuildingBlocks.ServiceIntegration.RabbitMQ;

public sealed class RabbitMQConnectionFactory
{
    private readonly RabbitMQOptions _config;
    
    public RabbitMQConnectionFactory(IOptions<RabbitMQOptions> config)
    {
        _config = config.Value;
    }

    public IConnection CreateConnection()
    {
        var factory = new ConnectionFactory { HostName = _config.HostName};

        return factory.CreateConnection();
    }
}
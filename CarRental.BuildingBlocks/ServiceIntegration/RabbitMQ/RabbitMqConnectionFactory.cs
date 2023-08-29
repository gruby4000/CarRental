using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CarRental.BuildingBlocks.ServiceIntegration.RabbitMQ;

public sealed class RabbitMqConnectionFactory
{
    private readonly RabbitMqOptions _config;
    
    public RabbitMqConnectionFactory(RabbitMqOptions config)
    {
        _config = config;
    }

    public IConnection CreateConnection()
    {
        var factory = new ConnectionFactory { HostName = _config.HostName};

        return factory.CreateConnection();
    }
}
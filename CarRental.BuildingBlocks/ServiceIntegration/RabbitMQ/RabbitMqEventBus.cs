using System.Text;
using System.Text.Json;
using MassTransit.RabbitMqTransport;
using MediatR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CarRental.BuildingBlocks.ServiceIntegration.RabbitMQ;

//refactor - IProducer, IConsumer
/// <summary>
/// Allows publishing and receiving messages in a simple way.
/// </summary>
public sealed class RabbitMqEventBus: IEventBus
{
    private readonly RabbitMqOptions _config;
    private readonly HashSet<RabbitMqEventConsumer<IEvent>> _consumers;
    private readonly HashSet<RabbitMqEventProducer<IEvent>> _producers; 
    private readonly IConnection _connection;
    private readonly RabbitMqConnectionFactory _connectionFactory;
    
    public RabbitMqEventBus(IOptions<RabbitMqOptions> config)
    {
        _consumers = new();
        _producers = new();
        _config = config.Value;
        _connectionFactory = new RabbitMqConnectionFactory(_config);
    }

    public void OpenConnection()
    {
        _connectionFactory.CreateConnection();
    }
    public void CloseConnection() 
    {
        _connection.Cleanup();    
    }

    public void Subscribe(IEventConsumer<IEvent> consumer)
    {
        if (consumer is null) throw new ArgumentNullException("Consumer cannot be null");
        var rabbitConsumer = consumer as RabbitMqEventConsumer<IEvent>;
        rabbitConsumer.SetConnection(_connection);
        rabbitConsumer.StartConsuming();
        _consumers.Add(rabbitConsumer);
    }

    public void Unsubscribe(string eventName)
    {
        var consumer = _consumers.SingleOrDefault(x => x.EventName.Equals(eventName));
        if (consumer is null) throw new ArgumentException("No consumer for event" + eventName);
        consumer.StopConsuming();
        _consumers.Remove(consumer);
    }

    public void RegisterProducer(RabbitMqEventProducer<IEvent> producer)
    {
        if (producer is null) throw new ArgumentNullException("Producer cannot be null");
        _producers.Add(producer);
    }

    public void UnregisterProducer(string eventName)
    {
        var producer = _producers.SingleOrDefault(x => x.EventName.Equals(eventName));
        if (producer is null) throw new ArgumentException("No producer of event" + eventName);
        _producers.Remove(producer);
    }

    public void Publish(IEvent @event)
    {
        var producer = _producers.SingleOrDefault(x => x.EventName.Equals(@event.EventName));
        if (producer is null) throw new ArgumentException("No producer of event" + @event.EventName);
        
        producer.Publish(@event);
    }

    public void PublishAll(params IEvent[] events)
    {
        foreach (IEvent message in events)
        {
            Publish(message);
        }
    }
    
    public void Dispose()
    {
        _connection.Cleanup();
    }
}
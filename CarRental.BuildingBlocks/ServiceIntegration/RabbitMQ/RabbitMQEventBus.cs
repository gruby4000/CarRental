using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CarRental.BuildingBlocks.ServiceIntegration.RabbitMQ;

/// <summary>
/// Simple implementation of pure RabbitMQEventBus without MassTransit features.
/// Allows publishing and receiving messages in a simple way. Not recommended for prod.
/// </summary>
public sealed class RabbitMQEventBus: IEventBus, IDisposable
{
    private readonly IMediator _mediator;
    private readonly RabbitMQOptions _config;
    public readonly HashSet<IEvent> _events;
    public readonly HashSet<IEvent> _subscribedEvents;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private EventingBasicConsumer _consumer;
    
    public RabbitMQEventBus(IOptions<RabbitMQOptions> config, IMediator mediator)
    {
        _mediator = mediator;
        _subscribedEvents = new();
        _config = config.Value;
        _events = new();

        var connFactory = new RabbitMQConnectionFactory(config);
        
        _connection = connFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(_config.Exchange, ExchangeType.Topic);
        _channel.QueueBind(_config.QueueName, _config.Exchange, _config.RoutingKey);
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += async (o, args) => await ReceiveMessage(o, args);

    }

    public Task PublishAllAsync(string routingKey = null!)
    {
        return Task.Run(() =>
        {
            var batch = _channel.CreateBasicPublishBatch();

            foreach (var @event in _events)
            {
                var properties = _channel.CreateBasicProperties();
                properties.AppId = _config.ServiceId;
                properties.Timestamp = new AmqpTimestamp();
                properties.MessageId = @event.CorrelationId.ToString();
                properties.Persistent = true;
                properties.CorrelationId = @event.CorrelationId.ToString();
                batch.Add(_config.Exchange,
                    routingKey != null ? routingKey : _config.RoutingKey,
                    true,
                    properties,
                    Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event)));
            }
        
            batch.Publish();
            _events.Clear();
        });
    }

    public Task PublishSpecificEventAsync(IEvent @event, string routingKey = null!)
    {
        return Task.Run(() =>
        {
            var properties = _channel.CreateBasicProperties();
            properties.AppId = _config.ServiceId;
            properties.Timestamp = new AmqpTimestamp();
            properties.MessageId = @event.CorrelationId.ToString();
            properties.Persistent = true;
            properties.CorrelationId = @event.CorrelationId.ToString();
        
            _channel.BasicPublish(
                _config.Exchange,
                routingKey != null ? routingKey : _config.RoutingKey,
                true,
                properties,
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event))
            );
        });
    }

    public void AddEventToPublish(IEvent @event)
    {
        _events.Add(@event);
    }

    public void SubscribeEvent(IEvent @event)
    {
        _subscribedEvents.Add(@event);
    }

    public void UnsubscribeEvent(IEvent @event)
    {
        _subscribedEvents.Remove(@event);
    }

    private async Task ReceiveMessage(object? sender, BasicDeliverEventArgs @event)
    {
        var bytes = @event.Body.ToArray();
        var json = JsonDocument.Parse(bytes);
        var typeName = json.RootElement.GetProperty("Type").GetString()!;
        
        if (!_subscribedEvents.Any(x => x.Type.Equals(typeName)))
            return;
        
        var serializedEvent = JsonSerializer.Deserialize(bytes, Type.GetType(typeName)!)!;
        await _mediator.Publish(serializedEvent);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
        this.Dispose();
    }
}
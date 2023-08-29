using System.Text.Json;
using MassTransit.Futures.Contracts;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CarRental.BuildingBlocks.ServiceIntegration.RabbitMQ;

public abstract class RabbitMqEventConsumer<T>: IEventConsumer<T> where T: IEvent
{
    private readonly RabbitMqOptions _config;
    private IConnection? _connection;
    private IModel? _channel;
    private EventingBasicConsumer? _consumer;
    private readonly List<Action<T>> _handlers;
    
    public required string QueueName { get; init; }
    public bool Durable { get; init; }
    public bool Exclusive { get; init; }
    public bool AutoDelete { get; init; }
    public List<string> Arguments { get; init; }
    public required string EventName { get; init; }
    
    public required string ExchangeType { get; init; }
    public string? RoutingKey { get; init; }
    public required string ExchangeName { get; init; }
    
    public RabbitMqEventConsumer()
    {
        _handlers = new();
        Arguments = new();
    }
    
    public void AddHandler(Action<T> handler)
    {
        _handlers.Add(handler);
    }

    public void SetConnection(IConnection connection)
    {
        if (_connection is null || !_connection.IsOpen)
            throw new ArgumentNullException("Connection must be initialized and open to set it in consumer");
        
        _connection = connection;
    }
    
    public virtual void StartConsuming()
    {
        if (_connection is null) throw new ("No connection was set");
        if (_channel is null) _channel = _connection.CreateModel();
        if (_consumer is null) _consumer = CreateConsumer(_channel);

        foreach (Action<T> handler in _handlers)
        {
            _consumer.Received += (sender, @event) =>
            {
                T convertedMessage = ConvertMessageToType(@event.Body.ToArray());
                handler(convertedMessage);
            };    
        }
        

        _channel.BasicConsume(QueueName, true, _consumer);
    }

    public virtual void StopConsuming()
    {
        _consumer = null;
    }
    
    protected virtual EventingBasicConsumer CreateConsumer(IModel channel)
    {
        if (_channel is null)
        {
            var ttl = new Dictionary<string, object>()
            {
                { "x-message-ttl", _config.Ttl.ToString() }
            };

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, ExchangeType, arguments: ttl);
            _channel.QueueBind(QueueName, ExchangeName, RoutingKey);
        }
        
        return new EventingBasicConsumer(_channel);
    }
    protected virtual T ConvertMessageToType(byte[] message)
    {
        var json = JsonDocument.Parse(message);
        var typeName = json.RootElement.GetProperty("Type").GetString()!;
        var serializedEvent = JsonSerializer.Deserialize<T>(message)!;
        
        return serializedEvent;
    }
}
using System.Globalization;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CarRental.BuildingBlocks.ServiceIntegration.RabbitMQ;

public abstract class RabbitMqEventProducer<T> where T: IEvent
{
    private readonly RabbitMqOptions _config;
    private IConnection _connection;
    private IModel _channel;
    
    public required string ExchangeType { get; init; }
    public string? RoutingKey { get; init; }
    public required string ExchangeName { get; init; }
    public required string EventName { get; init; }
    
    public RabbitMqEventProducer(RabbitMqOptions config)
    {
        _config = config;
    }

    public void SetConnection(IConnection connection)
    {
        if (_connection is null || !_connection.IsOpen)
            throw new ArgumentNullException("Connection must be initialized and open to set it in producer");
        
        _connection = connection;
    }
    
    public virtual void Publish(T message)
    {
        if(_connection is null) throw new ("No connection was set");
        
        var properties = _channel.CreateBasicProperties();
        properties.AppId = _config.ServiceId;
        properties.Timestamp = new AmqpTimestamp();
        properties.MessageId = message.CorrelationId.ToString();
        properties.Persistent = true;
        properties.CorrelationId = message.CorrelationId.ToString();

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        
        _channel.BasicPublish(ExchangeName, RoutingKey, true, properties, body);
    }
    
    protected virtual void CreatePublisher(IModel channel)
    {
        if (_channel is not null) return;
        
        var ttl = new Dictionary<string, object>()
        {
            { "x-message-ttl", _config.Ttl.ToString() }
        };

        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(ExchangeName, ExchangeType, arguments: ttl);
    }
}
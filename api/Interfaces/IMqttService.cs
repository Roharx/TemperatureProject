namespace api.Interfaces;

public interface IMqttService
{
    Task SubscribeAsync(string topic);
    Task UnsubscribeAsync(string topic);
    Task PublishAsync(string topic, string message);
}
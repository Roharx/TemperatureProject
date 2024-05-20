using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Service.Config;

namespace service.Services;

public class MqttService
{
    private readonly IMqttClient _mqttClient;
    private readonly ILogger<MqttService> _logger;
    private readonly MqttSettings _mqttSettings;

    public MqttService(ILogger<MqttService> logger, IConfiguration configuration)
    {
        _logger = logger;
        var factory = new MqttFactory();
        _mqttClient = factory.CreateMqttClient();
        _mqttSettings = configuration.GetSection("MqttSettings").Get<MqttSettings>();
    }

    public async Task StartAsync()
    {
        var options = new MqttClientOptionsBuilder()
            .WithClientId(_mqttSettings.ClientId)
            .WithTcpServer(_mqttSettings.BrokerUrl, _mqttSettings.Port)
            .WithCredentials(_mqttSettings.Username, _mqttSettings.Password)
            .Build();

        _mqttClient.UseConnectedHandler(async e =>
        {
            _logger.LogInformation("Connected to MQTT broker");
            await SubscribeAsync(_mqttSettings.Topic);
        });

        _mqttClient.UseDisconnectedHandler(e =>
        {
            _logger.LogInformation("Disconnected from MQTT broker");
            return Task.CompletedTask;
        });

        _mqttClient.UseApplicationMessageReceivedHandler(e =>
        {
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            _logger.LogInformation($"Received message: {message}");
        });

        await _mqttClient.ConnectAsync(options);
    }

    public async Task SubscribeAsync(string topic)
    {
        await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
        _logger.LogInformation($"Subscribed to topic {topic}");
    }

    public async Task UnsubscribeAsync(string topic)
    {
        await _mqttClient.UnsubscribeAsync(topic);
        _logger.LogInformation($"Unsubscribed from topic {topic}");
    }

    public async Task PublishAsync(string topic, string payload)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .Build();

        await _mqttClient.PublishAsync(message);
        _logger.LogInformation($"Published message to topic {topic}: {payload}");
    }
}
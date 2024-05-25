using System;
using System.Text;
using System.Text.Json;
using api.Config;
using api.DTOs.Office;
using api.DTOs.Room;
using api.Websockets;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using service.Interfaces;
using DeviceMessageDto = api.DTOs.Mqtt.DeviceMessageDto;
using UpdateTemperatureDto = api.DTOs.Mqtt.UpdateTemperatureDto;

namespace api.Services
{
    public class MqttService
    {
        private readonly IMqttClient _mqttClient;
        private readonly ILogger<MqttService> _logger;
        private readonly MqttSettings _mqttSettings;
        private readonly WebSocketServer _webSocketServer;
        private readonly ICrudService _crudService;

        public MqttService(ILogger<MqttService> logger, IConfiguration configuration, WebSocketServer webSocketServer, ICrudService crudService)
        {
            _logger = logger;
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _mqttSettings = configuration.GetSection("MqttSettings").Get<MqttSettings>();
            _webSocketServer = webSocketServer;
            _crudService = crudService;
        }

        public async Task StartAsync()
        {
            // Generate a unique client ID
            var clientId = $"{_mqttSettings.ClientId}_{Guid.NewGuid()}";

            var options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(_mqttSettings.BrokerUrl, _mqttSettings.Port)
                .WithCredentials(_mqttSettings.Username, _mqttSettings.Password)
                .Build();

            _mqttClient.UseConnectedHandler(async e =>
            {
                _logger.LogInformation("Connected to MQTT broker");
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("temp/#").Build());
                _logger.LogInformation("Subscribed to topic temp/#");
            });

            _mqttClient.UseDisconnectedHandler(e =>
            {
                _logger.LogInformation("Disconnected from MQTT broker");
            });

            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                _logger.LogInformation($"Received message: {message}");
                ProcessMqttMessage(e.ApplicationMessage.Topic, message);
            });

            await _mqttClient.ConnectAsync(options);
        }

        private void ProcessMqttMessage(string topic, string message)
        {
            // Print the message to the console
            Console.WriteLine($"Processing MQTT message: {message}");

            // Extract office name and room name from the topic
            var topicParts = topic.Split('/');
            if (topicParts.Length < 3 || topicParts[0] != "temp")
            {
                Console.WriteLine("Invalid topic format. Expected format: temp/officeName/roomName");
                return;
            }

            var officeName = topicParts[1];
            var roomName = topicParts[2];
            var fullRoomName = $"{officeName}/{roomName}";

            // Deserialize the message into DeviceMessageDto
            try
            {
                var deviceMessage = JsonSerializer.Deserialize<DeviceMessageDto>(message);
                if (deviceMessage != null)
                {
                    var updateData = new UpdateTemperatureDto(
                        name: roomName,
                        office_name: officeName,
                        desired_temp: deviceMessage.TargetTemperature,
                        window_toggle: deviceMessage.Toggle == 1
                    );

                    // Save data to the database
                    SaveTemperatureData(updateData);

                    // Post the message to the WebSocket server
                    _webSocketServer.Broadcast(fullRoomName, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing MQTT message: {ex.Message}");
            }
        }

        private void SaveTemperatureData(UpdateTemperatureDto updateData)
        {
            try
            {
                var officeDto = _crudService.GetSingleItemByParameters<GetOfficeDto>("office",
                    new Dictionary<string, object> { { "name", updateData.Office_name } });

                if (officeDto == null)
                {
                    throw new KeyNotFoundException($"Office with name '{updateData.Office_name}' not found.");
                }

                var officeId = officeDto.Id;

                var updateResult = _crudService.UpdateItem("room",
                    new Dictionary<string, object>
                    {
                        { "name", updateData.Name },
                        { "office_id", officeId }
                    },
                    new Dictionary<string, object>
                    {
                        { "desired_temp", updateData.Desired_temp },
                        { "window_toggle", updateData.Window_toggle }
                    });

                if (!updateResult)
                {
                    throw new Exception("Failed to update room settings.");
                }

                Console.WriteLine($"Updated temperature for room: {updateData.Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving temperature data: {ex.Message}");
            }
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
    }
}

using System.Text;
using System.Text.Json;
using api.Config;
using api.DTOs.Office;
using api.DTOs.Room;
using api.DTOs.Device;
using api.Interfaces;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using service.Interfaces;

namespace api.Services
{
    public class MqttService : IMqttService
    {
        private readonly IMqttClient _mqttClient;
        private readonly ILogger<MqttService> _logger;
        private readonly MqttSettings _mqttSettings;
        private readonly IWebSocketServer _webSocketServer;
        private readonly ICrudService _crudService;

        public MqttService(ILogger<MqttService> logger, IConfiguration configuration, IWebSocketServer webSocketServer,
            ICrudService crudService)
        {
            _logger = logger;
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _mqttSettings = configuration.GetSection("MqttSettings").Get<MqttSettings>();
            _mqttSettings.Username = Environment.GetEnvironmentVariable("MQTT_USERNAME");
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
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("config/#").Build());
                _logger.LogInformation("Subscribed to topic config/#");
            });

            _mqttClient.UseDisconnectedHandler(e => { _logger.LogInformation("Disconnected from MQTT broker"); });

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

            // Extract topic parts
            var topicParts = topic.Split('/');
            if (topicParts.Length < 2)
            {
                Console.WriteLine("Invalid topic format.");
                return;
            }

            if (topicParts[0] == "temp")
            {
                ProcessTemperatureMessage(topicParts, message);
            }
            else if (topicParts[0] == "config")
            {
                ProcessConfigMessage(topic, message).Wait();
            }
        }

        private void ProcessTemperatureMessage(string[] topicParts, string message)
        {
            // Extract office name and room name from the topic
            if (topicParts.Length < 3)
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

        private async Task ProcessConfigMessage(string topic, string message)
        {
            // Ignore messages that are responses sent by this service
            

            try
            {
                var startupMessage = JsonSerializer.Deserialize<StartupMessageDto>(message);
                

                if (startupMessage == null)
                {
                    _logger.LogError("Failed to deserialize the startup message. Deserialized object is null.");
                    return;
                }
                if (startupMessage.Message == "response")
                {
                    return;
                }

                _logger.LogInformation($"Deserialized startup message: {JsonSerializer.Serialize(startupMessage)}");

                if (string.IsNullOrEmpty(startupMessage.Message))
                {
                    _logger.LogError("Startup message 'Message' property is null or empty.");
                    return;
                }

                if (startupMessage.Message != "startup")
                {
                    _logger.LogError(
                        $"Startup message does not match 'startup'. Actual message: {startupMessage.Message}");
                    return;
                }

                _logger.LogInformation($"Processing startup message for device: {startupMessage.Device}");

                // Step 1: Get device information
                var device = _crudService.GetSingleItemByParameters<GetDeviceDto>("device",
                    new Dictionary<string, object> { { "id", startupMessage.Device } });
                if (device == null)
                {
                    _logger.LogError($"Device with ID {startupMessage.Device} not found.");
                    return;
                }

                // Step 2: Get office information
                var office = _crudService.GetSingleItemByParameters<GetOfficeDto>("office",
                    new Dictionary<string, object> { { "id", device.Office_id } });
                if (office == null)
                {
                    _logger.LogError($"Office with ID {device.Office_id} not found.");
                    return;
                }

                // Step 3: Get room information
                var room = _crudService.GetSingleItemByParameters<GetRoomDto>("room",
                    new Dictionary<string, object> { { "id", device.Room_id } });
                if (room == null)
                {
                    _logger.LogError($"Room with ID {device.Room_id} not found.");
                    return;
                }

                // Prepare response
                var response = new
                {
                    message = "response",
                    device = startupMessage.Device,
                    office = office.Name,
                    room = room.Name,
                    targetTemperature = room.Desired_temp,
                    humidityTreshold = 50.0,
                    humidityMax = 60.0,
                    toggle = room.Window_toggle
                };

                var responseMessage = JsonSerializer.Serialize(response);
                _logger.LogInformation($"Prepared response message: {responseMessage}");

                // Publish response message
                var responseTopic = $"config/";
                _logger.LogInformation($"Publishing response to topic: {responseTopic}");
                await PublishAsync(responseTopic, responseMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing config message: {ex.Message}");
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
            try
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .Build();

                await _mqttClient.PublishAsync(message);
                _logger.LogInformation($"Published message to topic {topic}: {payload}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error publishing message to topic {topic}: {ex.Message}");
            }
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
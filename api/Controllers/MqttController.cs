using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using api.DTOs.Mqtt;
using api.Services;
using api.Websockets;

namespace api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MqttController : ControllerBase
    {
        private readonly MqttService _mqttService;
        private readonly WebSocketServer _webSocketServer;

        public MqttController(MqttService mqttService, WebSocketServer webSocketServer)
        {
            _mqttService = mqttService;
            _webSocketServer = webSocketServer;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] string topic)
        {
            try
            {
                if (string.IsNullOrEmpty(topic))
                {
                    throw new ValidationException("Topic cannot be null or empty.");
                }

                await _mqttService.SubscribeAsync(topic);
                return Ok(new { Message = $"Subscribed to topic {topic}" });
            }
            catch (Exception ex)
            {
                throw new Exceptions.QueryExecutionException("Error subscribing to the topic.", ex);
            }
        }

        [HttpPost("unsubscribe")]
        public async Task<IActionResult> Unsubscribe([FromBody] string topic)
        {
            try
            {
                if (string.IsNullOrEmpty(topic))
                {
                    throw new ValidationException("Topic cannot be null or empty.");
                }

                await _mqttService.UnsubscribeAsync(topic);
                return Ok(new { Message = $"Unsubscribed from topic {topic}" });
            }
            catch (Exception ex)
            {
                throw new Exceptions.QueryExecutionException("Error unsubscribing from the topic.", ex);
            }
        }

        [HttpPost("postRoomSettings")]
        public async Task<IActionResult> PostRoomSettings([FromBody] RoomSettingsDto roomSettings)
        {
            try
            {
                if (roomSettings == null)
                {
                    throw new ValidationException("Room settings cannot be null.");
                }

                if (string.IsNullOrEmpty(roomSettings.Topic))
                {
                    throw new ValidationException("Topic cannot be null or empty.");
                }

                var message = JsonSerializer.Serialize(roomSettings);

                await _mqttService.PublishAsync(roomSettings.Topic, message);

                return Ok(new { Message = "Room settings message published successfully." });
            }
            catch (Exception ex)
            {
                throw new Exceptions.QueryExecutionException("Error publishing room settings message.", ex);
            }
        }
    }
}

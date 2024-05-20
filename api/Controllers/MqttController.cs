using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using exceptions;
using System.ComponentModel.DataAnnotations;
using service.Services;

namespace api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MqttController : ControllerBase
{
    private readonly MqttService _mqttService;

    public MqttController(MqttService mqttService)
    {
        _mqttService = mqttService;
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
}
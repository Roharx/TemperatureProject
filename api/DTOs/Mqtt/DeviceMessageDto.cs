using System.Text.Json.Serialization;

namespace api.DTOs.Mqtt;

public class DeviceMessageDto
{
    [JsonPropertyName("targetTemperature")]
    public float TargetTemperature { get; set; }

    [JsonPropertyName("humidityTreshold")]
    public float HumidityTreshold { get; set; }

    [JsonPropertyName("humidityMax")]
    public float HumidityMax { get; set; }

    [JsonPropertyName("toggle")]
    public int Toggle { get; set; }
}
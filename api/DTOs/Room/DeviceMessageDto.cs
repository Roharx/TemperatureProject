using System.Text.Json.Serialization;

namespace api.DTOs.Room;

public class DeviceMessageDto
{
    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("targetTemperature")]
    public int TargetTemperature { get; set; }

    [JsonPropertyName("humidityTreshold")]
    public int HumidityTreshold { get; set; }

    [JsonPropertyName("humidityMax")]
    public int HumidityMax { get; set; }

    [JsonPropertyName("toggle")]
    public int Toggle { get; set; }
}
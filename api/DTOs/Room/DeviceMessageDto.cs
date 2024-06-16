using System.Text.Json.Serialization;

namespace api.DTOs.Room;

public class DeviceMessageDto
{
    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("targetTemperature")]
    public double TargetTemperature { get; set; }

    [JsonPropertyName("humidityTreshold")]
    public double HumidityTreshold { get; set; }

    [JsonPropertyName("humidityMax")]
    public double HumidityMax { get; set; }

    [JsonPropertyName("toggle")]
    public int Toggle { get; set; }
}
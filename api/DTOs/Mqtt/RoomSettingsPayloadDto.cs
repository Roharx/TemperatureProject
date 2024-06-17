namespace api.DTOs.Mqtt;

public class RoomSettingsPayloadDto
{
    public string Source { get; set; }
    public double TargetTemperature { get; set; }
    public double HumidityThreshold { get; set; }
    public double HumidityMax { get; set; }
    public int Toggle { get; set; }
}
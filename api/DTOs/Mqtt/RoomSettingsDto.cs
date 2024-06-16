namespace api.DTOs.Mqtt
{
    public class RoomSettingsDto
    {
        public string Source { get; set; } = "server";
        public double TargetTemperature { get; set; }
        public double HumidityThreshold { get; set; }
        public double HumidityMax { get; set; }
        public int Toggle { get; set; }
        public string Topic { get; set; }
    }
}
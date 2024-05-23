namespace api.DTOs.Mqtt
{
    public class RoomSettingsDto
    {
        public string Source { get; set; } = "server";
        public int TargetTemperature { get; set; }
        public int HumidityThreshold { get; set; }
        public int HumidityMax { get; set; }
        public int Toggle { get; set; }
        public string Topic { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace api.DTOs.Device
{
    public class StartupMessageDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("device")]
        public int Device { get; set; }
    }
}
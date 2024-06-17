using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Mqtt;

public class RoomSettingsDto
{
    [Required]
    public string Source { get; set; }

    [Required]
    public int TargetTemperature { get; set; }

    [Required]
    public int HumidityThreshold { get; set; }

    [Required]
    public int HumidityMax { get; set; }

    [Required]
    public int Toggle { get; set; }

    [Required]
    public string Topic { get; set; }
}
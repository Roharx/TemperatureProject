namespace api.DTOs.Mqtt;

public class UpdateTemperatureDto
{
    public string Name { get; }
    public string Office_name { get; }
    public double Desired_temp { get; }
    public bool Window_toggle { get; }
    public double HumidityThreshold { get; }
    public double HumidityMax { get; }

    public UpdateTemperatureDto(string name, string office_name, double desired_temp, bool window_toggle, double humidityThreshold, double humidityMax)
    {
        Name = name;
        Office_name = office_name;
        Desired_temp = desired_temp;
        Window_toggle = window_toggle;
        HumidityThreshold = humidityThreshold;
        HumidityMax = humidityMax;
    }
}
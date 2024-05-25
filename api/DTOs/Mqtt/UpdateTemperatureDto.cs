namespace api.DTOs.Mqtt;

public class UpdateTemperatureDto
{
    public string Name { get; }
    public string Office_name { get; }
    public float Desired_temp { get; }
    public bool Window_toggle { get; }

    public UpdateTemperatureDto(string name, string office_name, float desired_temp, bool window_toggle)
    {
        Name = name;
        Office_name = office_name;
        Desired_temp = desired_temp;
        Window_toggle = window_toggle;
    }
}
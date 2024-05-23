namespace api.DTOs.Room
{
    public class UpdateTemperatureDto
    {
        public UpdateTemperatureDto(string name, string office_name, double desired_temp, bool window_toggle)
        {
            Name = name;
            Office_name = office_name;
            Desired_temp = desired_temp;
            Window_toggle = window_toggle;
        }

        public string Name { get; set; }
        public string Office_name { get; set; }
        public double Desired_temp { get; set; }
        public bool Window_toggle { get; set; }
    }
}
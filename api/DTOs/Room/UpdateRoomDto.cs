using System.Text.Json.Serialization;

namespace api.DTOs.Room
{
    public class UpdateRoomDto
    {
        [JsonConstructor]
        public UpdateRoomDto(int office_id, string name, bool window_toggle, double desired_temp, int req_rank)
        {
            Office_id = office_id;
            Name = name;
            Desired_temp = desired_temp;
            Window_toggle = window_toggle;
            Req_rank = req_rank;
        }

        public int Office_id { get; set; }
        public string Name { get; set; }
        public double Desired_temp { get; set; }
        public bool Window_toggle { get; set; }
        public int Req_rank { get; set; }
    }
}
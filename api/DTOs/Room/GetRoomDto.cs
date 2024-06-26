﻿namespace api.DTOs.Room
{
    public class GetRoomDto
    {
        public GetRoomDto() { } // for some reason dapper needs a parameterless constructor here

        public GetRoomDto(int id, int office_id, string name, bool physical_overlay_enabled, double desired_temp, 
            bool window_toggle, int req_rank)
        {
            Id = id;
            Office_id = office_id;
            Name = name;
            Physical_overlay_enabled = physical_overlay_enabled;
            Desired_temp = desired_temp;
            Window_toggle = window_toggle;
            Req_rank = req_rank;
        }

        public int Id { get; set; }
        public int Office_id { get; set; }
        public string Name { get; set; }
        public bool Physical_overlay_enabled { get; set; }
        public double Desired_temp { get; set; }
        public bool Window_toggle { get; set; }
        public int Req_rank { get; set; }
    }
}
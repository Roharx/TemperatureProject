namespace api.DTOs.Device;

public class AssignNameDto
{
    public AssignNameDto(string officeName, string roomName)
    {
        Office_name = officeName;
        Room_name = roomName;
    }

    public string Office_name { get; set; }
    public string Room_name { get; set; }
}
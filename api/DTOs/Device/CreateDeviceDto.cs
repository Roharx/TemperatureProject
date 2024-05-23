namespace api.DTOs.Device;

public class CreateDeviceDto
{
    public CreateDeviceDto(int officeId, int roomId)
    {
        Office_id = officeId;
        Room_id = roomId;
    }

    public int Office_id { get; set; }
    public int Room_id { get; set; }
}
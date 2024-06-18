namespace api.DTOs.Device;

public class GetDeviceDto
{
    public GetDeviceDto() { }

    public GetDeviceDto(int id, int officeId, int roomId)
    {
        Id = id;
        Office_id = officeId;
        Room_id = roomId;
    }

    public int Id { get; set; }
    public int Office_id { get; set; }
    public int Room_id { get; set; }
}
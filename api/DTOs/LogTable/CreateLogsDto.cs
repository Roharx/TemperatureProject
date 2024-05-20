namespace api.DTOs.LogTable;

public class CreateLogsDto
{
    public CreateLogsDto(string action, int room_id, int office_id, int account_id)
    {
        Action = action;
        Room_id = room_id;
        Office_id = office_id;
        Account_id = account_id;
    }
    public int Account_id { get; set; }
    public int Office_id { get; set; }
    public int Room_id { get; set; }
    public string Action { get; set; }
}
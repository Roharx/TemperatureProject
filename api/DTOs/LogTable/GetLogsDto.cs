namespace api.DTOs.LogTable;

public class GetLogsDto
{
    public GetLogsDto() {}
    public GetLogsDto(int id, string action, int room_id, int office_id, int account_id)
    {
        Id = id;
        Action = action;
        Room_id = room_id;
        Office_id = office_id;
        Account_id = account_id;
    }
    public int Id { get; set; }
    public int Account_id { get; set; }
    public int Office_id { get; set; }
    public int Room_id { get; set; }
    public string Action { get; set; }
}
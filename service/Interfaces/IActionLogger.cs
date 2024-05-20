namespace service.Interfaces;

public interface IActionLogger
{
    public void LogAction(int? accountId, int roomId, int officeId, string actionDescription);
}
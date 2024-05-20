using exceptions;
using service.Interfaces;

namespace service.Services
{
    public class ActionLogger : IActionLogger
    {
        private readonly ICrudService _service;
        public ActionLogger(ICrudService service)
        {
            _service = service;
        }
        public void LogAction(int? accountId, int roomId, int officeId, string actionDescription)
        {
            try
            {
                _service.CreateItem<bool>("log_table",
                    new Dictionary<string, object>
                    {
                        {"account_id", accountId},
                        {"office_id", officeId},
                        {"room_id", roomId},
                        {"action", actionDescription}
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exceptions.LoggingQueryExecutionException("Failed to log action.",e);
            }
        }
    }
}
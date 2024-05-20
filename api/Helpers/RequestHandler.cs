using api.DTOs;
using service.Interfaces;

namespace api.Helpers
{
    public class RequestHandler
    {
        private readonly IActionLogger _actionLogger;

        public RequestHandler(IActionLogger actionLogger)
        {
            _actionLogger = actionLogger;
        }

        public ResponseDto HandleRequest(Func<object?> action, int? accountId, int roomId, int officeId, string actionDescription)
        {
            try
            {
                var result = action();
                if (accountId != null)
                {
                    _actionLogger.LogAction(accountId,roomId,officeId, actionDescription);
                }
                return new ResponseDto
                {
                    MessageToClient = "Successfully executed action",
                    ResponseData = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    MessageToClient = ex.Message,
                    ResponseData = null
                };
            }
        }
    }
}
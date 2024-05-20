using api.DTOs;
using api.DTOs.LogTable;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;

namespace api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class LogsController : GenericControllerBase<ICrudService, GetLogsDto, CreateLogsDto, UpdateLogsDto>
{
    private readonly RequestHandler _requestHandler;
    public LogsController(ICrudService service, RequestHandler requestHandler) : base(service, "log_table")
    {
        _requestHandler = requestHandler;
    }

    [HttpPut]
    [Route("GetLogsForOffice")]
    public ResponseDto GetLogsForOffice([FromBody] int id)
    {
        return ValidateAndProceed(
            () => Service.GetSingleItemByParameters<GetLogsDto>(TableName, 
                new Dictionary<string, object> { { "office_id", id } }), 
            $"fetched logs for office: {id}");
    }
    
    [HttpPut]
    [Route("GetLogsForRoom")]
    public ResponseDto GetLogsForRoom([FromBody] int id)
    {
        return ValidateAndProceed(
            () => Service.GetSingleItemByParameters<GetLogsDto>(TableName, 
                new Dictionary<string, object> { { "room_id", id } }), 
            $"fetched logs for room: {id}");
    }
    
    [HttpPut]
    [Route("GetLogsForAccount")]
    public ResponseDto GetLogsForAccount([FromBody] int id)
    {
        return ValidateAndProceed(
            () => Service.GetSingleItemByParameters<GetLogsDto>(TableName, 
                new Dictionary<string, object> { { "account_id", id } }), 
            $"fetched logs for account: {id}");
    }
}
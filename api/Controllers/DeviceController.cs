using api.DTOs;
using api.DTOs.Device;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;

namespace api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class DeviceController : GenericControllerBase<ICrudService, GetDeviceDto, CreateDeviceDto, UpdateDeviceDto>
{
    private readonly RequestHandler _requestHandler;
    public DeviceController(ICrudService service, RequestHandler requestHandler) : base(service, "device")
    {
        _requestHandler = requestHandler;
    }

    [HttpPut]
    [Route("assignName/{id:int}")]
    public ResponseDto AssignName([FromRoute] int id)
    {
        return ValidateAndProceed(() => Service.GetSingleItemByParameters<GetDeviceDto>(TableName, 
                new Dictionary<string, object>
                {
                    {"id", id}
                }),
            $"fetched device information with id: {id}");
    }
    
}
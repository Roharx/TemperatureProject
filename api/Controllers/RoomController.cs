using api.DTOs;
using api.DTOs.Office;
using api.DTOs.Room;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;

namespace api.Controllers;

[Authorize]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
public class RoomController : GenericControllerBase<ICrudService, GetRoomDto, CreateRoomDto, UpdateRoomDto>
{
    private readonly RequestHandler _requestHandler;
    public RoomController(ICrudService service, RequestHandler requestHandler) : base(service, "room")
    {
        _requestHandler = requestHandler;
    }
    [HttpPut]
    [Route("getByName")]
    public ResponseDto GetRoomByName([FromBody] string name)
    {
        return ValidateAndProceed(
            () => Service.GetSingleItemByParameters<GetRoomDto>(TableName, 
                new Dictionary<string, object> { { "name", name } }), 
            $"fetched room with name {name}");
    }
}
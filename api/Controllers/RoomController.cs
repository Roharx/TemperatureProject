using api.DTOs;
using api.DTOs.Office;
using api.DTOs.Room;
using api.Helpers;
using exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;

namespace api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class RoomController : GenericControllerBase<ICrudService, GetRoomDto, CreateRoomDto, UpdateRoomDto>
{
    private readonly RequestHandler _requestHandler;

    public RoomController(ICrudService service, RequestHandler requestHandler)
        : base(service, "room")
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

    [HttpPut]
    [Route("getRoomsForOffice/{id:int}")]
    public ResponseDto GetRoomsForOffice([FromRoute] int id)
    {
        return ValidateAndProceed(
            () => Service.GetItemsByParameters<GetRoomDto>(TableName,
                new Dictionary<string, object> { { "office_id", id } }),
            $"fetched rooms for office: {id}");
    }

    [HttpPut]
    [Route("updateTemperature")]
    public ResponseDto UpdateTemperature([FromBody] UpdateTemperatureDto dto)
    {
        try
        {
            // Retrieve the office ID based on the office name
            var officeDto = Service.GetSingleItemByParameters<GetOfficeDto>("office",
                new Dictionary<string, object>
                {
                    { "name", dto.Office_name }
                });

            if (officeDto == null)
            {
                throw new KeyNotFoundException($"Office with name '{dto.Office_name}' not found.");
            }

            var officeId = officeDto.Id;

            // Update room settings
            var updateResult = Service.UpdateItem("room",
                new Dictionary<string, object>
                {
                    { "name", dto.Name },
                    { "office_id", officeId }
                },
                new Dictionary<string, object>
                {
                    { "desired_temp", dto.Desired_temp },
                    { "window_toggle", dto.Window_toggle }
                });

            if (!updateResult)
            {
                throw new Exceptions.QueryExecutionException("Failed to update room settings.", new Exception());
            }

            return new ResponseDto
            {
                MessageToClient = $"Updated temperature for room: {dto.Name}"
            };
        }
        catch (Exception ex)
        {
            throw new Exceptions.QueryExecutionException("Error updating temperature settings.", ex);
        }
    }
}

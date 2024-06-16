using api.DTOs;
using api.DTOs.Office;
using api.DTOs.Room;
using api.DTOs.Mqtt;
using api.Helpers;
using exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;
using System.Threading.Tasks;
using UpdateTemperatureDto = api.DTOs.Room.UpdateTemperatureDto;

namespace api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class RoomController : GenericControllerBase<ICrudService, GetRoomDto, CreateRoomDto, UpdateRoomDto>
{
    private readonly RequestHandler _requestHandler;
    private readonly MqttController _mqttController;

    public RoomController(ICrudService service, RequestHandler requestHandler, MqttController mqttController)
        : base(service, "room")
    {
        _requestHandler = requestHandler;
        _mqttController = mqttController;
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
    public async Task<ResponseDto> UpdateTemperature([FromBody] UpdateTemperatureDto dto)
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

            // Prepare the room settings message for MQTT
            var roomSettings = new RoomSettingsDto
            {
                Topic = $"office/{officeId}/room/{dto.Name}/settings",
                TargetTemperature = dto.Desired_temp,
                HumidityThreshold = 0, // Set appropriate value
                HumidityMax = 0,       // Set appropriate value
                Toggle = dto.Window_toggle ? 1 : 0
            };

            // Post the room settings message to the MQTT broker
            var mqttResult = await _mqttController.PostRoomSettings(roomSettings);

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

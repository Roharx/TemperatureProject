using System.Text.Json;
using api.DTOs;
using api.DTOs.Mqtt;
using api.DTOs.Office;
using api.DTOs.Room;
using api.Helpers;
using api.Interfaces;
using exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service.Interfaces;
using UpdateTemperatureDto = api.DTOs.Mqtt.UpdateTemperatureDto;

namespace api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RoomController : GenericControllerBase<ICrudService, GetRoomDto, CreateRoomDto, UpdateRoomDto>
    {
        private readonly RequestHandler _requestHandler;
        private readonly IMqttService _mqttService;

        public RoomController(ICrudService service, RequestHandler requestHandler, IMqttService mqttService)
            : base(service, "room")
        {
            _requestHandler = requestHandler;
            _mqttService = mqttService;
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

                // Map UpdateTemperatureDto to RoomSettingsPayloadDto
                var roomSettingsPayload = new RoomSettingsPayloadDto
                {
                    Source = "server", // or any appropriate value
                    TargetTemperature = dto.Desired_temp,
                    HumidityThreshold = dto.HumidityThreshold,
                    HumidityMax = dto.HumidityMax,
                    Toggle = dto.Window_toggle ? 1 : 0
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var message = JsonSerializer.Serialize(roomSettingsPayload, options);

                // Publish the message to the MQTT broker
                var topic = $"temp/{dto.Office_name}/{dto.Name}";
                await _mqttService.PublishAsync(topic, message);

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
}

using api.Controllers;
using api.DTOs.Office;
using api.DTOs.Room;
using api.Helpers;
using api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using service.Interfaces;
using Xunit;
using UpdateTemperatureDto = api.DTOs.Mqtt.UpdateTemperatureDto;

namespace tests.api
{
    public class RoomControllerTests
    {
        private readonly Mock<ICrudService> _mockService;
        private readonly Mock<IMqttService> _mockMqttService;
        private readonly RoomController _controller;

        public RoomControllerTests()
        {
            _mockService = new Mock<ICrudService>();
            _mockMqttService = new Mock<IMqttService>();
            var mockRequestHandler = new MockRequestHandler();
            _controller = new RoomController(_mockService.Object, mockRequestHandler, _mockMqttService.Object);

            // Mocking HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void GetAllItems_ShouldReturnAllItems()
        {
            // Arrange
            var rooms = new List<GetRoomDto> { new GetRoomDto() };
            _mockService.Setup(s => s.GetAllItems<GetRoomDto>("room")).Returns(rooms);

            // Act
            var result = _controller.GetAllItems();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Successfully fetched all items.", result.MessageToClient);
            Assert.Equal(rooms, result.ResponseData);
        }

        [Fact]
        public void GetItemById_ShouldReturnItem_WhenIdIsValid()
        {
            // Arrange
            var room = new GetRoomDto();
            _mockService.Setup(s => s.GetSingleItemByParameters<GetRoomDto>("room", It.IsAny<Dictionary<string, object>>())).Returns(room);

            // Act
            var result = _controller.GetItemById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Successfully fetched item with ID 1.", result.MessageToClient);
            Assert.Equal(room, result.ResponseData);
        }

        [Fact]
        public void GetRoomByName_ShouldReturnRoom_WhenNameIsValid()
        {
            // Arrange
            var room = new GetRoomDto();
            _mockService.Setup(s => s.GetSingleItemByParameters<GetRoomDto>("room", It.IsAny<Dictionary<string, object>>())).Returns(room);

            // Act
            var result = _controller.GetRoomByName("Conference Room");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Successfully fetched room with name Conference Room.", result.MessageToClient);
            Assert.Equal(room, result.ResponseData);
        }

        [Fact]
        public void GetRoomsForOffice_ShouldReturnRooms_WhenOfficeIdIsValid()
        {
            // Arrange
            var rooms = new List<GetRoomDto> { new GetRoomDto() };
            _mockService.Setup(s => s.GetItemsByParameters<GetRoomDto>("room", It.IsAny<Dictionary<string, object>>())).Returns(rooms);

            // Act
            var result = _controller.GetRoomsForOffice(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Successfully fetched rooms for office: 1.", result.MessageToClient);
            Assert.Equal(rooms, result.ResponseData);
        }

        [Fact]
        public void CreateItem_ShouldCreateRoom_WhenDtoIsValid()
        {
            // Arrange
            var createDto = new CreateRoomDto(1, "New Room", true, 22.5, false, 1);
            _mockService.Setup(s => s.CreateItem<GetRoomDto>("room", It.IsAny<Dictionary<string, object>>())).Returns(1);

            // Act
            var result = _controller.CreateItem(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Successfully created item.", result.MessageToClient);
            Assert.Equal(1, result.ResponseData);
        }

        [Fact]
        public void UpdateItem_ShouldUpdateRoom_WhenDtoIsValid()
        {
            // Arrange
            var updateDto = new UpdateRoomDto(1, "Updated Room", true, 22.5, 0);
            _mockService.Setup(s => s.UpdateItem("room", It.IsAny<Dictionary<string, object>>(), It.IsAny<Dictionary<string, object>>())).Returns(true);

            // Act
            var result = _controller.UpdateItem(1, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Successfully updated item.", result.MessageToClient);
        }

        [Fact]
        public void DeleteItem_ShouldDeleteRoom_WhenIdIsValid()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteItem("room", 1)).Returns(true);

            // Act
            var result = _controller.DeleteItem(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Successfully deleted item with given conditions.", result.MessageToClient);
        }

        [Fact]
        public async Task UpdateTemperature_ShouldUpdateRoomTemperature_WhenDtoIsValid()
        {
            // Arrange
            var updateTempDto = new UpdateTemperatureDto("Room1", "Office1", 23.0, true, 50.0, 60.0);
            var officeDto = new GetOfficeDto(1, "Office1", "Location1");
            var updateResult = true;

            _mockService.Setup(s => s.GetSingleItemByParameters<GetOfficeDto>("office", It.IsAny<Dictionary<string, object>>())).Returns(officeDto);
            _mockService.Setup(s => s.UpdateItem("room", It.IsAny<Dictionary<string, object>>(), It.IsAny<Dictionary<string, object>>())).Returns(updateResult);

            // Act
            var result = await _controller.UpdateTemperature(updateTempDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated temperature for room: Room1", result.MessageToClient);
        }
    }

    // Mock implementation of RequestHandler
    public class MockRequestHandler : RequestHandler
    {
        public MockRequestHandler() : base(null)
        {
        }
    }
}

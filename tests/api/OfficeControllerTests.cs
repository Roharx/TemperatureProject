using System;
using System.Collections.Generic;
using api.Controllers;
using api.DTOs;
using api.DTOs.Office;
using api.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using service.Interfaces;
using Xunit;

namespace tests.api
{
    public class OfficeControllerTests
    {
        private readonly Mock<ICrudService> _mockService;
        private readonly Mock<IActionLogger> _mockActionLogger;
        private readonly RequestHandler _requestHandler;
        private readonly OfficeController _controller;
        private int _nextId;

        public OfficeControllerTests()
        {
            _mockService = new Mock<ICrudService>();
            _mockActionLogger = new Mock<IActionLogger>();
            _requestHandler = new RequestHandler(_mockActionLogger.Object);
            _controller = new OfficeController(_mockService.Object, _requestHandler);

            // Mocking HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Initialize the ID counter
            _nextId = 0;
        }

        private int GetNextId()
        {
            Console.WriteLine($"Returning next ID: {_nextId}");
            _nextId++;
            return _nextId;
        }

        [Fact]
        public void GetOfficeByName_ReturnsCorrectOffice()
        {
            // Arrange
            var officeName = "Test Office";
            var officeDto = new GetOfficeDto(1, officeName, "Test Location");

            _mockService
                .Setup(service => service.GetSingleItemByParameters<GetOfficeDto>("office", It.IsAny<Dictionary<string, object>>()))
                .Returns(() => officeDto);

            // Act
            var result = _controller.GetOfficeByName(officeName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal($"Successfully fetched office with name {officeName}.", result.MessageToClient);
            Assert.Equal(officeDto, result.ResponseData);
        }

        [Fact]
        public void CreateOffice_ReturnsSuccessMessage()
        {
            // Arrange
            var createOfficeDto = new CreateOfficeDto("New Office", "New Location");

            _mockService
                .Setup(service => service.CreateItem<int>("office", It.IsAny<Dictionary<string, object>>()))
                .Returns(GetNextId);

            // Act
            var result = _controller.CreateItem(createOfficeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Successfully created item.", result.MessageToClient);
        }

        [Fact]
        public void UpdateOffice_ReturnsSuccessMessage()
        {
            // Arrange
            var updateOfficeDto = new UpdateOfficeDto("Updated Office", "Updated Location");

            _mockService
                .Setup(service => service.UpdateItem("office", It.IsAny<Dictionary<string, object>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns(() => true);

            // Act
            var result = _controller.UpdateItem(1, updateOfficeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Successfully updated item.", result.MessageToClient);
            Assert.True((bool)result.ResponseData);
        }

        [Fact]
        public void DeleteOffice_ReturnsSuccessMessage()
        {
            // Arrange
            _mockService
                .Setup(service => service.DeleteItem("office", It.IsAny<int>()))
                .Returns(() => true);

            // Act
            var result = _controller.DeleteItem(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Successfully deleted item with given conditions.", result.MessageToClient);
            Assert.True((bool)result.ResponseData);
        }

        [Fact]
        public void GetAllItems_ReturnsListOfOffices()
        {
            // Arrange
            var officeList = new List<GetOfficeDto>
            {
                new GetOfficeDto(1, "Office1", "Location1"),
                new GetOfficeDto(2, "Office2", "Location2")
            };

            _mockService
                .Setup(service => service.GetAllItems<GetOfficeDto>("office"))
                .Returns(() => officeList);

            // Act
            var result = _controller.GetAllItems();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Successfully fetched all items.", result.MessageToClient);
            var responseData = Assert.IsType<List<GetOfficeDto>>(result.ResponseData);
            Assert.Equal(2, responseData.Count);
        }

        [Fact]
        public void GetItemById_ReturnsCorrectOffice()
        {
            // Arrange
            var officeId = 1;
            var officeDto = new GetOfficeDto(officeId, "Office1", "Location1");

            _mockService
                .Setup(service => service.GetSingleItemByParameters<GetOfficeDto>("office", It.IsAny<Dictionary<string, object>>()))
                .Returns(() => officeDto);

            // Act
            var result = _controller.GetItemById(officeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal($"Successfully fetched item with ID {officeId}.", result.MessageToClient);
            Assert.Equal(officeDto, result.ResponseData);
        }
    }
}

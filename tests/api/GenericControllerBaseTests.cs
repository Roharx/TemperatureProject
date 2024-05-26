using api.Config;
using api.Controllers;
using api.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using service.Interfaces;
using Xunit;

namespace tests.api
{
    public class GenericControllerBaseTests
    {
        private readonly Mock<ICrudService> _mockService;
        private readonly GenericControllerBase<ICrudService, TestDto, TestDto, TestDto> _controller;

        public GenericControllerBaseTests()
        {
            _mockService = new Mock<ICrudService>();
            _controller = new GenericControllerBase<ICrudService, TestDto, TestDto, TestDto>(_mockService.Object, "testTable");

            // Mocking HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void GetAllItems_ShouldReturnSuccessResponse()
        {
            // Arrange
            var testData = new List<TestDto> { new TestDto { Name = "Item1" }, new TestDto { Name = "Item2" } };
            _mockService.Setup(s => s.GetAllItems<TestDto>("testTable")).Returns(testData);

            // Act
            var result = _controller.GetAllItems();

            // Assert
            Assert.Equal("Successfully fetched all items.", result.MessageToClient);
            Assert.Equal(testData, result.ResponseData);
        }

        [Fact]
        public void GetItemById_ShouldReturnSuccessResponse()
        {
            // Arrange
            var testItem = new TestDto { Name = "TestItem" };
            _mockService.Setup(s => s.GetSingleItemByParameters<TestDto>("testTable", It.IsAny<Dictionary<string, object>>()))
                        .Returns(testItem);

            // Act
            var result = _controller.GetItemById(1);

            // Assert
            Assert.Equal("Successfully fetched item with ID 1.", result.MessageToClient);
            Assert.Equal(testItem, result.ResponseData);
        }

        [Fact]
        public void CreateItem_ShouldReturnSuccessResponse()
        {
            // Arrange
            var newItem = new TestDto { Name = "NewItem" };
            _mockService.Setup(s => s.CreateItem<TestDto>("testTable", It.IsAny<Dictionary<string, object>>())).Returns(1);

            // Act
            var result = _controller.CreateItem(newItem);

            // Assert
            Assert.Equal("Successfully created item.", result.MessageToClient);
            Assert.Equal(1, result.ResponseData);
        }

        [Fact]
        public void UpdateItem_ShouldReturnSuccessResponse()
        {
            // Arrange
            var updateItem = new TestDto { Name = "UpdatedItem" };
            _mockService.Setup(s => s.UpdateItem("testTable", It.IsAny<Dictionary<string, object>>(), It.IsAny<Dictionary<string, object>>()))
                        .Returns(true);

            // Act
            var result = _controller.UpdateItem(1, updateItem);

            // Assert
            Assert.Equal("Successfully updated item.", result.MessageToClient);
            Assert.True((bool)result.ResponseData);
        }

        [Fact]
        public void DeleteItem_ShouldReturnSuccessResponse()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteItem("testTable", 1)).Returns(true);

            // Act
            var result = _controller.DeleteItem(1);

            // Assert
            Assert.Equal("Successfully deleted item with given conditions.", result.MessageToClient);
            Assert.True((bool)result.ResponseData);
        }
    }

    public class TestDto
    {
        public string Name { get; set; }
    }
}

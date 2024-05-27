using api.Config;
using api.Controllers;
using api.DTOs;
using api.DTOs.AccountOffice;
using api.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using service.Interfaces;
using Xunit;

namespace tests.api
{
    public class AccountOfficeControllerTests
    {
        private readonly Mock<ICrudService> _mockService;
        private readonly Mock<IActionLogger> _mockActionLogger;
        private readonly RequestHandler _requestHandler;
        private readonly AccountOfficeController _controller;

        public AccountOfficeControllerTests()
        {
            _mockService = new Mock<ICrudService>();
            _mockActionLogger = new Mock<IActionLogger>();
            _requestHandler = new RequestHandler(_mockActionLogger.Object);
            _controller = new AccountOfficeController(_mockService.Object, _requestHandler);

            // Mocking HttpContext to set up Request.Headers.Referer
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Referer"] = Whitelist.AllowedUrls.First(); // Use a URL from the whitelist
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void LinkAccountToOffice_ShouldReturnSuccessResponse()
        {
            // Arrange
            var dto = new CreateAccountOfficeDto(1, 2, 3);
            _mockService.Setup(s => s.CreateItemWithoutReturn("account_office", It.IsAny<Dictionary<string, object>>()))
                        .Returns(true);

            // Act
            var result = _controller.LinkAccountToOffice(dto);

            // Assert
            Assert.Equal("Successfully Linked account to office", result.MessageToClient.Trim().TrimEnd('.'));
            Assert.True((bool)result.ResponseData);
        }

        [Fact]
        public void GetAccountsForOffice_ShouldReturnSuccessResponse()
        {
            // Arrange
            var id = 1;
            var expectedItems = new List<GetAccountOfficeDto> { new GetAccountOfficeDto(1, 1, 3) };
            _mockService.Setup(s => s.GetItemsByParameters<GetAccountOfficeDto>("account_office", It.IsAny<Dictionary<string, object>>()))
                        .Returns(expectedItems);

            // Act
            var result = _controller.GetAccountsForOffice(id);

            // Assert
            Assert.Equal($"Successfully Fetched all accounts for office: {id}", result.MessageToClient.Trim().TrimEnd('.'));
            Assert.Equal(expectedItems, result.ResponseData);
        }

        [Fact]
        public void GetOfficesForAccount_ShouldReturnSuccessResponse()
        {
            // Arrange
            var id = 1;
            var expectedItems = new List<GetAccountOfficeDto> { new GetAccountOfficeDto(1, 1, 3) };
            _mockService.Setup(s => s.GetItemsByParameters<GetAccountOfficeDto>("account_office", It.IsAny<Dictionary<string, object>>()))
                        .Returns(expectedItems);

            // Act
            var result = _controller.GetOfficesForAccount(id);

            // Assert
            Assert.Equal($"Successfully Fetched all offices for account: {id}", result.MessageToClient.Trim().TrimEnd('.'));
            Assert.Equal(expectedItems, result.ResponseData);
        }

        [Fact]
        public void RemoveAccountFromOffice_ShouldReturnSuccessResponse()
        {
            // Arrange
            var dto = new DeleteAccountOfficeDto(1, 2);
            _mockService.Setup(s => s.DeleteItemWithMultipleParams("account_office", It.IsAny<Dictionary<string, object>>()))
                        .Returns(true);

            // Act
            var result = _controller.RemoveAccountFromOffice(dto);

            // Assert
            Assert.Equal("Successfully Removed account from office", result.MessageToClient.Trim().TrimEnd('.'));
            Assert.True((bool)result.ResponseData);
        }

        [Fact]
        public void ModifyAccountRankForOffice_ShouldReturnSuccessResponse()
        {
            // Arrange
            var dto = new UpdateAccountOfficeDto(1, 2, 4);
            _mockService.Setup(s => s.UpdateItem("account_office", It.IsAny<Dictionary<string, object>>(), It.IsAny<Dictionary<string, object>>()))
                        .Returns(true);

            // Act
            var result = _controller.ModifyAccountRankForOffice(dto);

            // Assert
            Assert.Equal("Successfully Updated account rank", result.MessageToClient.Trim().TrimEnd('.'));
            Assert.True((bool)result.ResponseData);
        }

        [Fact]
        public void GetAllItems_ShouldReturnSuccessResponse()
        {
            // Arrange
            var expectedItems = new List<GetAccountOfficeDto> { new GetAccountOfficeDto(1, 1, 3) };
            _mockService.Setup(s => s.GetAllItems<GetAccountOfficeDto>("account_office"))
                        .Returns(expectedItems);

            // Act
            var result = _controller.GetAllItems();

            // Assert
            Assert.Equal("Successfully fetched all items", result.MessageToClient.Trim().TrimEnd('.'));
            Assert.Equal(expectedItems, result.ResponseData);
        }

        [Fact]
        public void GetItemById_ShouldReturnSuccessResponse()
        {
            // Arrange
            var id = 1;
            var expectedItem = new GetAccountOfficeDto(1, 1, 3);
            _mockService.Setup(s => s.GetSingleItemByParameters<GetAccountOfficeDto>("account_office", It.IsAny<Dictionary<string, object>>()))
                        .Returns(expectedItem);

            // Act
            var result = _controller.GetItemById(id);

            // Assert
            Assert.Equal($"Successfully fetched item with ID {id}", result.MessageToClient.Trim().TrimEnd('.'));
            Assert.Equal(expectedItem, result.ResponseData);
        }
    }
}

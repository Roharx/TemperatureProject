using api.Controllers;
using api.DTOs;
using api.DTOs.Account;
using api.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using service.Interfaces;
using Xunit;

namespace tests.api
{
    public class AccountControllerTests
    {
        private readonly Mock<ICrudService> _mockService;
        private readonly Mock<IActionLogger> _mockActionLogger;
        private readonly RequestHandler _requestHandler;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _mockService = new Mock<ICrudService>();
            _mockActionLogger = new Mock<IActionLogger>();
            _requestHandler = new RequestHandler(_mockActionLogger.Object);
            _controller = new AccountController(_mockService.Object, _requestHandler);

            // Mocking HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void GetAccountByName_ShouldReturnSuccessResponse()
        {
            // Arrange
            var accountDto = new GetAccountDto { Id = 1, Name = "Test", Email = "test@example.com" };
            var getAccountByNameDto = new GetAccountByNameDto { Name = "Test" };
            _mockService.Setup(s => s.GetSingleItemByParameters<GetAccountDto>("account", It.IsAny<Dictionary<string, object>>()))
                        .Returns(accountDto);

            // Act
            var result = _controller.GetAccountByName(getAccountByNameDto);

            // Assert
            Assert.Equal("Successfully fetched account with name Test.", result.MessageToClient);
            Assert.Equal(accountDto, result.ResponseData);
        }

        [Fact]
        public void GetAccountByEmail_ShouldReturnSuccessResponse()
        {
            // Arrange
            var accountDto = new GetAccountDto { Id = 1, Name = "Test", Email = "test@example.com" };
            var getAccountByEmailDto = new GetAccountByEmailDto { Email = "test@example.com" };
            _mockService.Setup(s => s.GetSingleItemByParameters<GetAccountDto>("account", It.IsAny<Dictionary<string, object>>()))
                        .Returns(accountDto);

            // Act
            var result = _controller.GetAccountByEmail(getAccountByEmailDto);

            // Assert
            Assert.Equal("Successfully fetched account with email test@example.com.", result.MessageToClient);
            Assert.Equal(accountDto, result.ResponseData);
        }

        [Fact]
        public void Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var loginDto = new LoginDto("TestUser", "Password123");
            var token = "valid-token";
            _mockService.Setup(s => s.VerifyLogin(loginDto.Username, loginDto.Password)).Returns(token);

            // Act
            var result = _controller.Login(loginDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var responseDto = result.Value as ResponseDto;
            Assert.Equal(token, responseDto.ResponseData);
        }

        [Fact]
        public void Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginDto = new LoginDto("TestUser", "WrongPassword");
            _mockService.Setup(s => s.VerifyLogin(loginDto.Username, loginDto.Password)).Returns((string)null);

            // Act
            var result = _controller.Login(loginDto) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            var responseDto = result.Value as ResponseDto;
            Assert.Equal("Wrong username or password", responseDto.MessageToClient);
        }

        [Fact]
        public void Register_ShouldReturnSuccessResponse()
        {
            // Arrange
            var createAccountDto = new CreateAccountDto("TestUser", "Password123", "test@example.com");
            _mockService.Setup(s => s.CreateAccount(It.IsAny<Dictionary<string, object>>())).Returns(1);

            // Act
            var result = _controller.Register(createAccountDto);

            // Assert
            Assert.Equal("Successfully executed registry action.", result.MessageToClient);
            Assert.Equal(1, result.ResponseData);
        }

        [Fact]
        public void GetAccountByName_ShouldReturnErrorResponse_WhenExceptionOccurs()
        {
            // Arrange
            var getAccountByNameDto = new GetAccountByNameDto { Name = "Test" };
            _mockService.Setup(s => s.GetSingleItemByParameters<GetAccountDto>("account", It.IsAny<Dictionary<string, object>>()))
                        .Throws(new Exception("Test Exception"));

            // Act
            var result = _controller.GetAccountByName(getAccountByNameDto);

            // Assert
            Assert.Equal("An error occurred while processing your request.", result.MessageToClient);
            Assert.Null(result.ResponseData);
        }

        [Fact]
        public void GetAllItems_ShouldReturnSuccessResponse()
        {
            // Arrange
            var accountDtos = new List<GetAccountDto> { new GetAccountDto { Id = 1, Name = "Test", Email = "test@example.com" } };
            _mockService.Setup(s => s.GetAllItems<GetAccountDto>("account")).Returns(accountDtos);

            // Act
            var result = _controller.GetAllItems();

            // Assert
            Assert.Equal("Successfully fetched all items.", result.MessageToClient);
            Assert.Equal(accountDtos, result.ResponseData);
        }
    }
}

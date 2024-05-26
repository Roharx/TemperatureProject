using infrastructure.DataModels;
using infrastructure.Interfaces;
using Moq;
using service.Interfaces;
using service.Services;
using Xunit;

namespace tests.service
{
    public class CrudServiceTests
    {
        private readonly Mock<ICrudHandler> _mockCrudHandler;
        private readonly Mock<IHashService> _mockHashService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly CrudService _crudService;

        public CrudServiceTests()
        {
            _mockCrudHandler = new Mock<ICrudHandler>();
            _mockHashService = new Mock<IHashService>();
            _mockTokenService = new Mock<ITokenService>();
            _crudService = new CrudService(_mockCrudHandler.Object, _mockHashService.Object, _mockTokenService.Object);
        }

        [Fact]
        public void GetAllItems_ShouldReturnItems()
        {
            // Arrange
            var expectedItems = new List<TestModel> { new TestModel { Id = 1, Name = "Test" } };
            _mockCrudHandler.Setup(ch => ch.GetAllItems<TestModel>(It.IsAny<string>())).Returns(expectedItems);

            // Act
            var result = _crudService.GetAllItems<TestModel>("test_table");

            // Assert
            Assert.Equal(expectedItems, result);
        }

        [Fact]
        public void GetItemsByParameters_ShouldReturnItems()
        {
            // Arrange
            var parameters = new Dictionary<string, object> { { "Id", 1 } };
            var expectedItems = new List<TestModel> { new TestModel { Id = 1, Name = "Test" } };
            _mockCrudHandler.Setup(ch => ch.GetItemsByParameters<TestModel>(It.IsAny<string>(), parameters)).Returns(expectedItems);

            // Act
            var result = _crudService.GetItemsByParameters<TestModel>("test_table", parameters);

            // Assert
            Assert.Equal(expectedItems, result);
        }

        [Fact]
        public void GetSingleItemByParameters_ShouldReturnItem()
        {
            // Arrange
            var parameters = new Dictionary<string, object> { { "Id", 1 } };
            var expectedItem = new TestModel { Id = 1, Name = "Test" };
            _mockCrudHandler.Setup(ch => ch.GetSingleItemByParameters<TestModel>(It.IsAny<string>(), parameters)).Returns(expectedItem);

            // Act
            var result = _crudService.GetSingleItemByParameters<TestModel>("test_table", parameters);

            // Assert
            Assert.Equal(expectedItem, result);
        }

        [Fact]
        public void CreateItem_ShouldReturnNewId()
        {
            // Arrange
            var parameters = new Dictionary<string, object> { { "Name", "Test" } };
            var newId = 1;
            _mockCrudHandler.Setup(ch => ch.CreateItem(It.IsAny<string>(), parameters)).Returns(newId);

            // Act
            var result = _crudService.CreateItem<TestModel>("test_table", parameters);

            // Assert
            Assert.Equal(newId, result);
        }

        [Fact]
        public void CreateItemWithoutReturn_ShouldReturnTrue()
        {
            // Arrange
            var parameters = new Dictionary<string, object> { { "Name", "Test" } };
            _mockCrudHandler.Setup(ch => ch.CreateItemWithoutReturn(It.IsAny<string>(), parameters)).Returns(true);

            // Act
            var result = _crudService.CreateItemWithoutReturn("test_table", parameters);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateItem_ShouldReturnTrue()
        {
            // Arrange
            var conditionColumns = new Dictionary<string, object> { { "Id", 1 } };
            var modifications = new Dictionary<string, object> { { "Name", "UpdatedTest" } };
            _mockCrudHandler.Setup(ch => ch.UpdateItem(It.IsAny<string>(), conditionColumns, modifications)).Returns(true);

            // Act
            var result = _crudService.UpdateItem("test_table", conditionColumns, modifications);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteItem_ShouldReturnTrue()
        {
            // Arrange
            _mockCrudHandler.Setup(ch => ch.DeleteItem(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            // Act
            var result = _crudService.DeleteItem("test_table", 1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteItemWithMultipleParams_ShouldReturnTrue()
        {
            // Arrange
            var conditionColumns = new Dictionary<string, object> { { "Id", 1 } };
            _mockCrudHandler.Setup(ch => ch.DeleteItemWithMultipleParams(It.IsAny<string>(), conditionColumns)).Returns(true);

            // Act
            var result = _crudService.DeleteItemWithMultipleParams("test_table", conditionColumns);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyLogin_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";
            var account = new AccountVerification { Id = 1, Name = "testuser", Email = "testuser@example.com", Password = "hashedpassword" };
            _mockCrudHandler.Setup(ch => ch.GetSingleItemByParameters<AccountVerification>(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                            .Returns(account);
            _mockHashService.Setup(hs => hs.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var token = "generated_token";
            _mockTokenService.Setup(ts => ts.GenerateToken(It.IsAny<Account>())).Returns(token);

            // Act
            var result = _crudService.VerifyLogin(username, password);

            // Assert
            Assert.Equal(token, result);
        }

        [Fact]
        public void VerifyLogin_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var username = "testuser";
            var password = "wrongpassword";
            var account = new AccountVerification { Id = 1, Name = "testuser", Email = "testuser@example.com", Password = "hashedpassword" };
            _mockCrudHandler.Setup(ch => ch.GetSingleItemByParameters<AccountVerification>(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                            .Returns(account);
            _mockHashService.Setup(hs => hs.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = _crudService.VerifyLogin(username, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateAccount_ShouldReturnNewAccountId()
        {
            // Arrange
            var accountData = new Dictionary<string, object>
            {
                { "name", "testuser" },
                { "password", "testpassword" },
                { "email", "testuser@example.com" }
            };
            var hashedPassword = "hashedpassword";
            _mockHashService.Setup(hs => hs.HashPassword(It.IsAny<string>())).Returns(hashedPassword);
            _mockCrudHandler.Setup(ch => ch.CreateItem(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).Returns(1);

            // Act
            var result = _crudService.CreateAccount(accountData);

            // Assert
            Assert.Equal(1, result);
            Assert.Equal(hashedPassword, accountData["password"]);
        }

        [Fact]
        public void UpdateAccount_ShouldReturnTrue()
        {
            // Arrange
            var conditionColumns = new Dictionary<string, object> { { "Id", 1 } };
            var modifications = new Dictionary<string, object> { { "Password", "newpassword" } };
            var hashedPassword = "hashednewpassword";
            _mockHashService.Setup(hs => hs.HashPassword(It.IsAny<string>())).Returns(hashedPassword);
            _mockCrudHandler.Setup(ch => ch.UpdateItem(It.IsAny<string>(), conditionColumns, modifications)).Returns(true);

            // Act
            var result = _crudService.UpdateAccount(conditionColumns, modifications);

            // Assert
            Assert.True(result);
            Assert.Equal(hashedPassword, modifications["Password"]);
        }
    }

    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

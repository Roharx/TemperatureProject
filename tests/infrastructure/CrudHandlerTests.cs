using System.Collections.Generic;
using System.Data;
using exceptions;
using infrastructure.Interfaces;
using infrastructure.Repositories;
using Moq;
using Npgsql;
using Xunit;

namespace tests.infrastructure
{
    public class CrudHandlerTests
    {
        private readonly Mock<IDatabase> _mockDatabase;
        private readonly CrudHandler _crudHandler;

        public CrudHandlerTests()
        {
            _mockDatabase = new Mock<IDatabase>();
            _crudHandler = new CrudHandler(_mockDatabase.Object);
        }

        [Fact]
        public void GetAllItems_ShouldReturnItems()
        {
            // Arrange
            var expectedItems = new List<TestModel> { new TestModel { Id = 1, Name = "Test" } };
            _mockDatabase.Setup(db => db.Query<TestModel>(It.IsAny<string>(), null, null, true, null, null))
                           .Returns(expectedItems);

            // Act
            var result = _crudHandler.GetAllItems<TestModel>("test_table");

            // Assert
            Assert.Equal(expectedItems, result);
        }

        [Fact]
        public void GetItemsByParameters_ShouldReturnItems()
        {
            // Arrange
            var parameters = new Dictionary<string, object> { { "Id", 1 } };
            var expectedItems = new List<TestModel> { new TestModel { Id = 1, Name = "Test" } };
            _mockDatabase.Setup(db => db.Query<TestModel>(It.IsAny<string>(), parameters, null, true, null, null))
                           .Returns(expectedItems);

            // Act
            var result = _crudHandler.GetItemsByParameters<TestModel>("test_table", parameters);

            // Assert
            Assert.Equal(expectedItems, result);
        }

        [Fact]
        public void GetSingleItemByParameters_ShouldReturnItem()
        {
            // Arrange
            var parameters = new Dictionary<string, object> { { "Id", 1 } };
            var expectedItem = new TestModel { Id = 1, Name = "Test" };
            _mockDatabase.Setup(db => db.QueryFirstOrDefault<TestModel>(It.IsAny<string>(), parameters, null, null, null))
                           .Returns(expectedItem);

            // Act
            var result = _crudHandler.GetSingleItemByParameters<TestModel>("test_table", parameters);

            // Assert
            Assert.Equal(expectedItem, result);
        }

        [Fact]
        public void CreateItem_ShouldReturnNewId()
        {
            // Arrange
            var parameters = new Dictionary<string, object> { { "Name", "Test" } };
            var newId = 1;
            _mockDatabase.Setup(db => db.ExecuteScalar<int>(It.IsAny<string>(), parameters, null, null, null))
                           .Returns(newId);

            // Act
            var result = _crudHandler.CreateItem("test_table", parameters);

            // Assert
            Assert.Equal(newId, result);
        }

        [Fact]
        public void CreateItemWithoutReturn_ShouldReturnTrue()
        {
            // Arrange
            var parameters = new Dictionary<string, object> { { "Name", "Test" } };

            // Act
            var result = _crudHandler.CreateItemWithoutReturn("test_table", parameters);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateItem_ShouldReturnTrue()
        {
            // Arrange
            var conditionColumns = new Dictionary<string, object> { { "Id", 1 } };
            var modifications = new Dictionary<string, object> { { "Name", "UpdatedTest" } };

            // Act
            var result = _crudHandler.UpdateItem("test_table", conditionColumns, modifications);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteItem_ShouldReturnTrue()
        {
            // Act
            var result = _crudHandler.DeleteItem("test_table", 1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteItemWithMultipleParams_ShouldReturnTrue()
        {
            // Arrange
            var conditionColumns = new Dictionary<string, object> { { "Id", 1 } };

            // Act
            var result = _crudHandler.DeleteItemWithMultipleParams("test_table", conditionColumns);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ExecuteDbOperation_ShouldThrowDatabaseConnectionException_OnNpgsqlException()
        {
            // Arrange
            _mockDatabase.Setup(db => db.Query<TestModel>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<bool>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .Throws(new NpgsqlException());

            // Act & Assert
            Assert.Throws<Exceptions.DatabaseConnectionException>(() =>
                _crudHandler.GetAllItems<TestModel>("test_table"));
        }

        [Fact]
        public void ExecuteDbOperation_ShouldThrowQueryExecutionException_OnException()
        {
            // Arrange
            _mockDatabase.Setup(db => db.Query<TestModel>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<bool>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .Throws(new Exception());

            // Act & Assert
            Assert.Throws<Exceptions.QueryExecutionException>(() =>
                _crudHandler.GetAllItems<TestModel>("test_table"));
        }
    }

    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

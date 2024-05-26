using service.Services;
using Xunit;

namespace tests.service
{
    public class HashServiceTests
    {
        private readonly HashService _hashService;

        public HashServiceTests()
        {
            _hashService = new HashService();
        }

        [Fact]
        public void HashPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            var password = "TestPassword";

            // Act
            var hashedPassword = _hashService.HashPassword(password);

            // Assert
            Assert.False(string.IsNullOrEmpty(hashedPassword));
            Assert.NotEqual(password, hashedPassword);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_ForValidPassword()
        {
            // Arrange
            var password = "TestPassword";
            var hashedPassword = _hashService.HashPassword(password);

            // Act
            var result = _hashService.VerifyPassword(hashedPassword, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForInvalidPassword()
        {
            // Arrange
            var password = "TestPassword";
            var wrongPassword = "WrongPassword";
            var hashedPassword = _hashService.HashPassword(password);

            // Act
            var result = _hashService.VerifyPassword(hashedPassword, wrongPassword);

            // Assert
            Assert.False(result);
        }
    }
}
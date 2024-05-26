using System.IdentityModel.Tokens.Jwt;
using System.Text;
using api.Middleware;
using infrastructure.DataModels;
using Microsoft.IdentityModel.Tokens;
using service.Services;
using Xunit;

namespace tests.service
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public TokenServiceTests()
        {
            _jwtSettings = new JwtSettings
            {
                Key = "SuperSecretKey!1234567890123456MustBeVeryLongToBeGreaterThan256bits",
                Issuer = "testIssuer",
                Audience = "testAudience",
                ExpirationMinutes = 60,
                Pepper = "randomPepper"
            };

            _tokenService = new TokenService(_jwtSettings);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidToken()
        {
            // Arrange
            var account = new Account(1, "testUser", "testUser@example.com");

            // Act
            var token = _tokenService.GenerateToken(account);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key))
            };

            // Validate token
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            var jwtToken = validatedToken as JwtSecurityToken;

            Assert.NotNull(jwtToken);
            Assert.Equal(_jwtSettings.Issuer, jwtToken.Issuer);
            Assert.Equal(_jwtSettings.Audience, jwtToken.Audiences.First());
            Assert.Contains(jwtToken.Claims, c => c.Type == "id" && c.Value == account.Id.ToString());
        }
    }
}

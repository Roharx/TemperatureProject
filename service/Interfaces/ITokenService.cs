using infrastructure.DataModels;

namespace service.Interfaces;

public interface ITokenService
{
    string? GenerateToken(Account account);
}
namespace service.Interfaces;

public interface IHashService
{
    /// <summary>
    /// Hashes the raw string password to TODO:(develop which hash).
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    string HashPassword(string password);
    /// <summary>
    /// Verifies the raw password input with the hashed password and returns true if they match.
    /// </summary>
    /// <param name="hashedPassword"></param>
    /// <param name="rawPassword"></param>
    /// <returns></returns>
    bool VerifyPassword(string hashedPassword, string rawPassword);
}
using CryptSharp;
using service.Interfaces;

namespace service.Services;

public class HashService : IHashService
{
    public string HashPassword(string password)
    {
        var salt = Crypter.Blowfish.GenerateSalt(); //implement salting later
        var hashed = Crypter.Blowfish.Crypt(password, salt);
        return hashed;
    }

    public bool VerifyPassword(string hashedPassword, string rawPassword)
    {
        return Crypter.CheckPassword(rawPassword, hashedPassword);
    }
}
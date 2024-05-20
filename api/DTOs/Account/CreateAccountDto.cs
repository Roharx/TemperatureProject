using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Account;

public class CreateAccountDto
{
    public CreateAccountDto(string name, string password, string email)
    {
        Name = name;
        Email = email;
        Password = password;
    }

    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}
namespace api.DTOs.Account
{
    public class UpdateAccountDto
    {
        public UpdateAccountDto(string name, string password, string email)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        

        
    }
}
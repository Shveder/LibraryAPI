namespace Library.Core.DTO;

public class RegisterUserRequest
{
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public string PasswordRepeat { get; set; }
}
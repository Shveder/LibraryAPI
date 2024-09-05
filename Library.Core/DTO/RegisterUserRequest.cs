namespace Library.Core.DTO;

public class RegisterUserRequest
{
    /// <summary>
    /// User login to register
    /// </summary>
    public string Login { get; set; }
    
    /// <summary>
    /// User password to register
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// User password repeat to register
    /// </summary>
    public string PasswordRepeat { get; set; }
}
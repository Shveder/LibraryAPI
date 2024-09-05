namespace Library.Core.DTO;

public class LoginRequest
{
    /// <summary>
    /// User login to authorize
    /// </summary>
    public string Login { get; set; }
    
    /// <summary>
    /// User password to authorize
    /// </summary>
    public string Password { get; set; }
}
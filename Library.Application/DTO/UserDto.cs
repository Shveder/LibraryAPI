namespace Library.Application.DTO;

public class UserDto : BaseDto
{
    /// <summary>
    /// Login of user
    /// </summary>
    public string Login { get; set; } 
    
    /// <summary>
    /// Password of user
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Salt of user password
    /// </summary>
    public string Salt { get; set; } 
    
    /// <summary>
    /// Role of user 
    /// </summary>
    public string Role { get; set; }
}
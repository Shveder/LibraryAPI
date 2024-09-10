namespace Library.API.Controllers;

/// <summary>
/// Controller responsible for user authorization, including login and registration functionalities.
/// </summary>
/// <param name="authorizationService">Service handling authorization logic such as generating tokens and user registration.</param>
[ApiController]
[Route("[controller]")]
public class AuthorizationController(IAuthorizationService authorizationService) : ControllerBase
{
    /// <summary>
    /// Logs in a user by validating credentials and generating a JWT token.
    /// </summary>
    /// <param name="request">The login request containing user credentials.</param>
    /// <returns>
    /// A JWT token if the login is successful.
    /// </returns>
    [HttpPost("Login")]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await authorizationService.GenerateTokenAsync(request.Login, request.Password);
        
        return Ok(token);
    }

    /// <summary>
    /// Registers a new user with the provided information.
    /// </summary>
    /// <param name="request">The registration request containing user details.</param>
    /// <returns>
    /// A success message if registration is successful.
    /// </returns>
    [HttpPost("Register")]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        await authorizationService.Register(request);
        
        return Ok("Registration successful");
    }
}
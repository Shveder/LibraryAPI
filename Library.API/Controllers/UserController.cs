namespace Library.API.Controllers;

/// <summary>
/// 
/// </summary>
/// <param name="userService"></param>
[Route("api/User")]
[ApiController]
public class UserController(IUserService authorService)
    : BaseController<IUserService, User, UserDto>(authorService);

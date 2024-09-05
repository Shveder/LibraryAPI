namespace Library.API.Controllers;

/// <summary>
/// Controller responsible for managing users, including operations such as retrieving, creating, updating, and deleting users.
/// </summary>
/// <param name="userService">Service that handles operations related to user management.</param>
[Route("api/User")]
[ApiController]
public class UserController(IUserService userService)
    : BaseController<IUserService, User, UserDto>(userService);
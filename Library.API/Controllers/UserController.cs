namespace Library.API.Controllers;

/// <summary>
/// Controller responsible for managing users, including operations such as retrieving, creating, updating, and deleting users.
/// </summary>
[Route("api/User")]
[ApiController]
public class UserController(GetUserByIdUseCase getUserByIdUseCase) : ControllerBase
{
    /// <summary>
    /// Retrieves a user by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>An IActionResult containing the requested user.</returns>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var entity = await getUserByIdUseCase.GetByIdAsync(id);
        
        return Ok(new ResponseDto<UserDto>(CommonStrings.SuccessResult, data: entity));
    }
}
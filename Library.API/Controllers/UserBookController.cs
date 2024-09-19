namespace Library.API.Controllers;

/// <summary>
/// Controller responsible for managing the relationship between users and their books.
/// </summary>
[Route("api/UserBook")]
[ApiController]
public class UserBookController(PostUserBookUseCase postUseCase,
    DeleteUserBookUseCase deleteUseCase, GetBookByUserUseCase getByUserUseCase)
    : ControllerBase
{
    /// <summary>
    /// Retrieves the list of books associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>
    /// A list of books associated with the user.
    /// </returns>
    [HttpGet]
    [Route("GetBooksByUserId")]
    [ProducesResponseType(typeof(IEnumerable<UserBookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBooksByUserId(Guid userId)
    {
        var bookList = await getByUserUseCase.GetBooksByUserId(userId);
        
        return Ok(bookList);
    }

    /// <summary>
    /// Creates a new user-book association.
    /// </summary>
    /// <param name="dto">The DTO containing the details of the user-book association.</param>
    /// <returns>
    /// The created user-book association.
    /// </returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto<UserBookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostAsync(UserBookDto dto)
    {
        var entity = await postUseCase.PostAsync(dto);
        
        return Ok(new ResponseDto<UserBookDto>(CommonStrings.SuccessResultPost, data: entity));
    }
    
    /// <summary>
    /// Deleting userBooks by its id
    /// </summary>
    /// <param name="id">The unique identifier of the userBook to delete</param>
    /// <returns>An IActionResult containing the success data</returns>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        await deleteUseCase.DeleteByIdAsync(id);
        
        return Ok(new ResponseDto<string>(CommonStrings.SuccessResultDelete));
    }
}
namespace Library.API.Controllers;

/// <summary>
/// Controller for managing authors.
/// Inherits common CRUD operations from the BaseController.
/// </summary>
[Route("api/Author")]
[ApiController]
public class AuthorController(
    PostAuthorUseCase postUseCase,
    GetAllAuthorsUseCase getAllUseCase,
    GetAuthorByIdUseCase getByIdUseCase,
    DeleteAuthorByIdUseCase deleteUseCase) : ControllerBase
{
    /// <summary>
    /// Deleting author by its id
    /// </summary>
    /// <param name="id">The unique identifier of the author to delete</param>
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

    /// <summary>
    /// Retrieves an author by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the author to retrieve.</param>
    /// <returns>An IActionResult containing the requested author.</returns>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var entity = await getByIdUseCase.GetByIdAsync(id);
        
        return Ok(new ResponseDto<AuthorDto>(CommonStrings.SuccessResult, data: entity));
    }

    /// <summary>
    /// Creates a new author.
    /// </summary>
    /// <param name="dto">The DTO containing the information of the author to create.</param>
    /// <returns>An IActionResult containing the created author.</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> PostAsync(AuthorDto dto)
    {
        var entity = await postUseCase.PostAsync(dto);
        
        return Ok(new ResponseDto<AuthorDto>(CommonStrings.SuccessResultPost, data: entity));
    }

    /// <summary>
    /// Retrieves all authors.
    /// </summary>
    /// <returns>An IActionResult containing the list of all authors.</returns>
    [HttpGet("GetAll")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetAllAsync()
    {
        var entity = await getAllUseCase.GetAllAsync();
        
        return Ok(new ResponseDto<IEnumerable<AuthorDto>>(CommonStrings.SuccessResult, data: entity));
    }
}
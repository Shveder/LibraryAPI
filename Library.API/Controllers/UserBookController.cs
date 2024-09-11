namespace Library.API.Controllers;

/// <summary>
/// Controller responsible for managing the relationship between users and their books.
/// </summary>
[Route("api/UserBook")]
[ApiController]
public class UserBookController(IPostUserBookUseCase postUseCase,
    IGetAllUserBooksUseCase getAllUseCase, IGetUserBookByIdUseCase getByIdUseCase,
    IDeleteUserBookUseCase deleteUseCase, IPutUserBookUseCase putUseCase, IGetBookByUserUseCase getByUserUseCase)
    : BaseController<IPostUserBookUseCase, IGetAllUserBooksUseCase, IGetUserBookByIdUseCase, IDeleteUserBookUseCase,
        IPutUserBookUseCase, UserBook, UserBookDto>(postUseCase, getAllUseCase, getByIdUseCase, deleteUseCase, putUseCase)
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
    public override async Task<IActionResult> PostAsync(UserBookDto dto)
    {
        var entity = await postUseCase.PostAsync(dto);
        
        return Ok(new ResponseDto<UserBookDto>(CommonStrings.SuccessResultPost, data: entity));
    }
}
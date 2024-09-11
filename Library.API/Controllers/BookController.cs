namespace Library.API.Controllers;

/// <summary>
/// Controller responsible for managing book-related operations, including fetching books by ISBN, author, and with filters.
/// </summary>
[Route("api/Book")]
[ApiController]
public class BookController(IPostBookUseCase postUseCase,
    IGetAllBooksUseCase getAllUseCase, IGetBookByIdUseCase getByIdUseCase,
    IDeleteBookUseCase deleteUseCase, IPutBookUseCase putUseCase,
    IGetBookByIsbnUseCase getBookByIsbnUseCase, IGetByAuthorUseCase getByAuthorUseCase,
    IGetAllFilteredBooks getAllFilteredBooks)
    : BaseController<IPostBookUseCase, IGetAllBooksUseCase, IGetBookByIdUseCase, IDeleteBookUseCase,
        IPutBookUseCase, Book, BookDto>(postUseCase, getAllUseCase, getByIdUseCase, deleteUseCase, putUseCase)
{
    /// <summary>
    /// Retrieves a book by its ISBN.
    /// </summary>
    /// <param name="isbn">The ISBN of the book.</param>
    /// <returns>
    /// The book details corresponding to the provided ISBN.
    /// </returns>
    [HttpGet("GetBookByIsbn")]
    [ProducesResponseType(typeof(ResponseDto<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIsbnAsync(string isbn)
    {
        var bookDto = await getBookByIsbnUseCase.GetByIsbnAsync(isbn);
        return Ok(new ResponseDto<BookDto>(CommonStrings.SuccessResult, data: bookDto));
    }

    /// <summary>
    /// Retrieves books by a specific author.
    /// </summary>
    /// <param name="authorId">The ID of the author.</param>
    /// <returns>
    /// A list of books written by the specified author.
    /// </returns>
    [HttpGet("GetByAuthor")]
    [ProducesResponseType(typeof(ResponseDto<IEnumerable<BookDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByAuthor(Guid authorId)
    {
        var bookDtos = await getByAuthorUseCase.GetByAuthor(authorId);
        return Ok(new ResponseDto<IEnumerable<BookDto>>(CommonStrings.SuccessResult, data: bookDtos));
    }

    /// <summary>
    /// Retrieves a list of books with filtering options applied.
    /// </summary>
    /// <param name="filter">The filter criteria for retrieving books.</param>
    /// <returns>
    /// A list of books and the total count based on the applied filters.
    /// </returns>
    [HttpGet("GetAllFiltered")]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFilteredAsync([FromQuery] FilterDto filter)
    {
        var (books, totalCount) = await getAllFilteredBooks.GetAllFiltered(filter);
        var response = new
        {
            Books = books,
            TotalCount = totalCount
        };

        return Ok(new ResponseDto<object>(CommonStrings.SuccessResult, data: response));
    }

    /// <summary>
    /// Retrieves a book by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the book.</param>
    /// <returns>
    /// The book details corresponding to the provided ID.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseDto<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public override async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var bookDto = await getByIdUseCase.GetByIdAsync(id);
        return Ok(new ResponseDto<BookDto>(CommonStrings.SuccessResult, data: bookDto));
    }

    /// <summary>
    /// Creates a new book.
    /// </summary>
    /// <param name="dto">The DTO containing the information of the book to create.</param>
    /// <returns>
    /// The created book details.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseDto<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
    public override async Task<IActionResult> PostAsync([FromBody] BookDto dto)
    {
        var bookDto = await postUseCase.PostAsync(dto);
        return Ok(new ResponseDto<BookDto>(CommonStrings.SuccessResultPost, data: bookDto));
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <param name="dto">The DTO containing the updated information of the book.</param>
    /// <returns>
    /// The updated book details.
    /// </returns>
    [HttpPut]
    [ProducesResponseType(typeof(ResponseDto<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
    public override async Task<IActionResult> PutAsync([FromBody] BookDto dto)
    {
        var bookDto = await putUseCase.PutAsync(dto);
        return Ok(new ResponseDto<BookDto>(CommonStrings.SuccessResultPut, data: bookDto));
    }

    /// <summary>
    /// Deletes a book by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the book to delete.</param>
    /// <returns>
    /// A success message if the book is deleted.
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResponseDto<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public override async Task<IActionResult> DeleteAsync(Guid id)
    {
        await deleteUseCase.DeleteByIdAsync(id);
        return Ok(new ResponseDto<string>(CommonStrings.SuccessResultDelete));
    }
}
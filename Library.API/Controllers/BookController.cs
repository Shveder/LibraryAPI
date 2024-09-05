namespace Library.API.Controllers;

/// <summary>
/// Controller responsible for managing book-related operations, including fetching books by ISBN, author, and with filters.
/// </summary>
/// <param name="bookService">Service handling book-related operations.</param>
[Route("api/Book")]
[ApiController]
public class BookController (IBookService bookService)
    : BaseController<IBookService, Book, BookDto>(bookService){
    private readonly IBookService _bookService = bookService;

    /// <summary>
    /// Retrieves a book by its ISBN.
    /// </summary>
    /// <param name="isbn">The ISBN of the book.</param>
    /// <returns>
    /// The book details corresponding to the provided ISBN.
    /// </returns>
    [HttpGet]
    [Route("GetBookByIsbn")]
    [ProducesResponseType(typeof(ResponseDto<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIsbnAsync(string isbn)
    {
        var entity = await _bookService.GetByIsbnAsync(isbn);
        
        return Ok(new ResponseDto<BookDto>(CommonStrings.SuccessResult, data: entity));
    }

    /// <summary>
    /// Retrieves books by a specific author.
    /// </summary>
    /// <param name="authorId">The ID of the author.</param>
    /// <returns>
    /// A list of books written by the specified author.
    /// </returns>
    [HttpGet]
    [Route("GetByAuthor")]
    [ProducesResponseType(typeof(ResponseDto<IEnumerable<BookDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByAuthor(Guid authorId)
    {
        var entity = await _bookService.GetByAuthor(authorId);
        
        return Ok(new ResponseDto<IEnumerable<BookDto>>(CommonStrings.SuccessResult, data: entity));
    }
    
    /// <summary>
    /// Retrieves a list of books with filtering options applied.
    /// </summary>
    /// <param name="filter">The filter criteria for retrieving books.</param>
    /// <returns>
    /// A list of books and the total count based on the applied filters.
    /// </returns>
    [HttpGet]
    [Route("GetAllFiltered")]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetFilteredAsync([FromQuery] FilterDto filter)
    {
        var (books, totalCount) = await _bookService.GetAllFiltered(filter);
        var response = new
        {
            Books = books,
            TotalCount = totalCount
        };

        return Ok(new ResponseDto<object>(CommonStrings.SuccessResult, data: response));
    }
}
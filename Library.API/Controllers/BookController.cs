namespace Library.API.Controllers;

/// <summary>
/// 
/// </summary>
/// <param name="bookService"></param>
[Route("api/Book")]
[ApiController]
public class BookController (IBookService bookService)
    : BaseController<IBookService, Book, BookDto>(bookService){
    private readonly IBookService _bookService = bookService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isbn"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetBookByIsbn")]
    public async Task<IActionResult> GetByIsbnAsync(string isbn)
    {
        var entity = await _bookService.GetByIsbnAsync(isbn);
        
        return Ok(new ResponseDto<BookDto>(CommonStrings.SuccessResult, data: entity));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="authorId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetByAuthor")]
    public async Task<IActionResult> GetByAuthor(Guid authorId)
    {
        var entity = await _bookService.GetByAuthor(authorId);
        
        return Ok(new ResponseDto<IEnumerable<BookDto>>(CommonStrings.SuccessResult, data: entity));
    }
    
    /// <summary>
    /// Получает список сущностей с применением фильтрации.
    /// </summary>
    /// <param name="filter">Фильтр для получения сущностей.</param>
    /// <returns>Список DTO сущностей.</returns>
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
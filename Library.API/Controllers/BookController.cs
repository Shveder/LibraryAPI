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
}


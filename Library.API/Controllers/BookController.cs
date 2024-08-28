using Library.API.Controllers.Base;
using Library.Common;
using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

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
}


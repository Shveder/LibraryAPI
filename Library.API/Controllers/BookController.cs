using Library.API.Controllers.Base;
using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

/// <summary>
/// 
/// </summary>
/// <param name="bookService"></param>
[Route("api/book")]
[ApiController]
public class BookController (IBookService bookService)
    : BaseController<IBookService, Book, BookDto>(bookService);

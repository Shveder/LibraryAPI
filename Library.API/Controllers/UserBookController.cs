using Library.API.Controllers.Base;
using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/UserBook")]
[ApiController]
public class UserBookController(IUserBookService bookService)
    : BaseController<IUserBookService, UserBook, UserBookDto>(bookService);

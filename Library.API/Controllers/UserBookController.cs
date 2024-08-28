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
public class UserBookController(IUserBookService userBookService)
    : BaseController<IUserBookService, UserBook, UserBookDto>(userBookService)
{
    private readonly IUserBookService _userBookService = userBookService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetBooksByUserId")]
    public async Task<IActionResult> GetBooksByUserId(Guid userId)
    {
        var bookList = await _userBookService.GetBooksByUserId(userId);
        return Ok(bookList);
    }
}

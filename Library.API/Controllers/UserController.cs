using Library.API.Controllers.Base;
using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

/// <summary>
/// 
/// </summary>
/// <param name="userService"></param>
[Route("api/User")]
[ApiController]
public class UserController(IUserService authorService)
    : BaseController<IUserService, User, UserDto>(authorService);

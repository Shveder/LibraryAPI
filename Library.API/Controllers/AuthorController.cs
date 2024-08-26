using Library.API.Controllers.Base;
using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

/// <summary>
/// 
/// </summary>
/// <param name="authorService"></param>
[Route("api/author")]
[ApiController]
public class AuthorController(IAuthorService authorService)
    : BaseController<IAuthorService, Author, AuthorDto>(authorService);

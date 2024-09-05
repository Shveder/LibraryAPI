namespace Library.API.Controllers;

/// <summary>
/// 
/// </summary>
/// <param name="authorService"></param>
[Route("api/Author")]
[ApiController]
public class AuthorController(IAuthorService authorService)
    : BaseController<IAuthorService, Author, AuthorDto>(authorService);
namespace Library.API.Controllers;

/// <summary>
/// Controller for managing authors.
/// Inherits common CRUD operations from the BaseController.
/// </summary>
/// <param name="authorService">The service responsible for handling author-related operations.</param>
[Route("api/Author")]
[ApiController]
public class AuthorController(IAuthorService authorService)
    : BaseController<IAuthorService, Author, AuthorDto>(authorService);
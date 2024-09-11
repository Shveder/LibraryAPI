namespace Library.API.Controllers;

/// <summary>
/// Controller for managing authors.
/// Inherits common CRUD operations from the BaseController.
/// </summary>
[Route("api/Author")]
[ApiController]
public class AuthorController(IPostAuthorUseCase postUseCase,
    IGetAllAuthorsUseCase getAllUseCase, IGetAuthorByIdUseCase getByIdUseCase,
    IDeleteAuthorByIdUseCase deleteUseCase, IPutAuthorUseCase putUseCase)
    : BaseController<IPostAuthorUseCase, IGetAllAuthorsUseCase, IGetAuthorByIdUseCase, IDeleteAuthorByIdUseCase,
        IPutAuthorUseCase, Author, AuthorDto>(postUseCase, getAllUseCase, getByIdUseCase, deleteUseCase, putUseCase);
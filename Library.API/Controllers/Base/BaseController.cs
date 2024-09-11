namespace Library.API.Controllers.Base;

/// <summary>
/// Base controller providing CRUD operations for entities.
/// </summary>
/// <typeparam name="TEntity">The entity type representing the database model.</typeparam>
/// <typeparam name="TEntityDto">The data transfer object (DTO) type used for transferring data.</typeparam>
/// <typeparam name="TPostUseCase"></typeparam>
/// <typeparam name="TGetAllUseCase"></typeparam>
/// <typeparam name="TPutUseCase"></typeparam>
/// <typeparam name="TGetByIdUseCase"></typeparam>
/// <typeparam name="TDeleteUseCase"></typeparam>
[Route("api/[controller]")]
public class BaseController<TPostUseCase, TGetAllUseCase, TGetByIdUseCase, TDeleteUseCase, TPutUseCase, TEntity, TEntityDto>
    (TPostUseCase postUseCase, TGetAllUseCase getAllUseCase, TGetByIdUseCase getByIdUseCase,
        TDeleteUseCase deleteUseCase, TPutUseCase putUseCase) : Controller
    where TPostUseCase : IPostUseCase<TEntityDto, TEntity>
    where TGetAllUseCase : IGetAllUseCase<TEntityDto, TEntity>
    where TGetByIdUseCase : IGetByIdUseCase<TEntityDto, TEntity>
    where TDeleteUseCase : IDeleteByIdUseCase<TEntityDto, TEntity>
    where TPutUseCase : IPutUseCase<TEntityDto, TEntity>
    where TEntityDto : BaseDto
    where TEntity : BaseModel
{
    private readonly TPostUseCase _postUseCase = postUseCase;
    private readonly TGetAllUseCase _getAllUseCase = getAllUseCase;
    private readonly TGetByIdUseCase _getByIdUseCase = getByIdUseCase;
    private readonly TDeleteUseCase _deleteUseCase = deleteUseCase;
    private TPutUseCase _putUseCase = putUseCase;
    

    /// <summary>
    /// Deleting entity by its id
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete</param>
    /// <returns>An IActionResult containing the success data</returns>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _deleteUseCase.DeleteByIdAsync(id);
        
        return Ok(new ResponseDto<string>(CommonStrings.SuccessResultDelete));
    }

    /// <summary>
    /// Retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>An IActionResult containing the requested entity.</returns>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var entity = await _getByIdUseCase.GetByIdAsync(id);
        
        return Ok(new ResponseDto<TEntityDto>(CommonStrings.SuccessResult, data: entity));
    }

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="dto">The DTO containing the updated information of the entity.</param>
    /// <returns>An IActionResult containing the updated entity.</returns>
    [HttpPut]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> PutAsync(TEntityDto dto)
    {
        var entity = await _putUseCase.PutAsync(dto);
        
        return Ok(new ResponseDto<TEntityDto>(CommonStrings.SuccessResultPut, data: entity));
    }

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <param name="dto">The DTO containing the information of the entity to create.</param>
    /// <returns>An IActionResult containing the created entity.</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> PostAsync(TEntityDto dto)
    {
        var entity = await _postUseCase.PostAsync(dto);
        
        return Ok(new ResponseDto<TEntityDto>(CommonStrings.SuccessResultPost, data: entity));
    }

    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>An IActionResult containing the list of all entities.</returns>
    [HttpGet("GetAll")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> GetAllAsync()
    {
        var entity = await _getAllUseCase.GetAllAsync();
        
        return Ok(new ResponseDto<IEnumerable<TEntityDto>>(CommonStrings.SuccessResult, data: entity));
    }
}
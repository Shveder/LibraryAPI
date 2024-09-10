namespace Library.API.Controllers.Base;

/// <summary>
/// Base controller providing CRUD operations for entities.
/// </summary>
/// <typeparam name="TService">The service type that handles the business logic.</typeparam>
/// <typeparam name="TEntity">The entity type representing the database model.</typeparam>
/// <typeparam name="TEntityDto">The data transfer object (DTO) type used for transferring data.</typeparam>
/// <param name="service">The service instance used for handling operations.</param>
[Route("api/[controller]")]
public class BaseController<TService, TEntity, TEntityDto>(TService service) : Controller
    where TService : IBaseService<TEntityDto, TEntity>
    where TEntityDto : BaseDto
    where TEntity : BaseModel
{
    private readonly TService _service = service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _service.DeleteByIdAsync(id);
        
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
        var entity = await _service.GetByIdAsync(id);
        
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
        var entity = await _service.PutAsync(dto);
        
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
        var entity = await _service.PostAsync(dto);
        
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
        var entity = await _service.GetAllAsync();
        
        return Ok(new ResponseDto<IEnumerable<TEntityDto>>(CommonStrings.SuccessResult, data: entity));
    }
}
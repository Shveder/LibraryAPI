namespace Library.API.Controllers.Base;

/// <summary>
/// 
/// </summary>
/// <param name="service"></param>
/// <typeparam name="TService"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityDto"></typeparam>
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
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _service.DeleteByIdAsync(id);
        
        return Ok(new ResponseDto<string>(CommonStrings.SuccessResultDelete));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [Authorize]
    public virtual async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var entity = await _service.GetByIdAsync(id);
        
        return Ok(new ResponseDto<TEntityDto>(CommonStrings.SuccessResult, data: entity));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize(Roles = "admin")]
    public virtual async Task<IActionResult> PutAsync(TEntityDto dto)
    {
        var entity = await _service.PutAsync(dto);
        
        return Ok(new ResponseDto<TEntityDto>(CommonStrings.SuccessResultPut, data: entity));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public virtual async Task<IActionResult> PostAsync(TEntityDto dto)
    {
        var entity = await _service.PostAsync(dto);
        
        return Ok(new ResponseDto<TEntityDto>(CommonStrings.SuccessResultPost, data: entity));
    }

    /// <summary>
    /// Get all entities.
    /// </summary>
    /// <returns>List of entities.</returns>
    [HttpGet("GetAll")]
    [Authorize]
    public virtual async Task<IActionResult> GetAllAsync()
    {
        var entity = await _service.GetAllAsync();
        
        return Ok(new ResponseDto<IEnumerable<TEntityDto>>(CommonStrings.SuccessResult, data: entity));
    }
}
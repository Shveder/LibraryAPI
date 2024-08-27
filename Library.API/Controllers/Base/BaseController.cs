using Library.Common;
using Library.Core.DTO;
using Library.Core.DTO.Base;
using Library.Core.Models.Base;
using Library.Infrastructure.Services.Base;
using Microsoft.AspNetCore.Mvc;

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
    [HttpDelete("{id:int}")]
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
    [HttpGet("{id:int}")]
    public virtual async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var entity = await _service.GetByIdAsync(id);
        
        return Ok(new ResponseDto<TEntityDto>(CommonStrings.SuccessResult, data: entity));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> PutAsync(Guid id, TEntityDto dto)
    {
        var entity = await _service.PutAsync(id, dto);
        
        return Ok(new ResponseDto<TEntityDto>(CommonStrings.SuccessResultPut, data: entity));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
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
    public virtual async Task<IActionResult> GetAllAsync()
    {
        var entity = await _service.GetAllAsync();
        return Ok(new ResponseDto<IEnumerable<TEntityDto>>(CommonStrings.SuccessResult, data: entity));
    }
}
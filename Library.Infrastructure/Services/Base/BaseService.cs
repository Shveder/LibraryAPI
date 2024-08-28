using AutoInterfaceAttributes;
using AutoMapper;
using Library.Common;
using Library.Core.DTO.Base;
using Library.Core.Models.Base;
using Library.Infrastructure.Exceptions;
using Library.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Services.Base;

[AutoInterface]
public class BaseService<TDto, TEntity>(IDbRepository dbRepository, IMapper mapper) : IBaseService<TDto, TEntity>
    where TDto : BaseDto
    where TEntity : BaseModel
{
    protected readonly IMapper Mapper = mapper;

    public virtual async Task<TDto> GetByIdAsync(Guid id)
    {
        var entity = await dbRepository.Get<TEntity>(e => e.Id == id).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);

        var dto = Mapper.Map<TDto>(entity);
        return dto;
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var entities = await dbRepository.GetAll<TEntity>().ToListAsync();
        var dtos = Mapper.Map<IEnumerable<TDto>>(entities);
        return dtos;
    }

    public virtual async Task DeleteByIdAsync(Guid id)
    {
        var entity = await dbRepository.Get<TEntity>(e => e.Id == id).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException($"{typeof(TEntity).Name} {CommonStrings.NotFoundResult}");

        await dbRepository.Delete<TEntity>(id);
        await dbRepository.SaveChangesAsync();
    }

    public virtual async Task<TDto> PostAsync(TDto dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        await dbRepository.Add(entity);
        await dbRepository.SaveChangesAsync();
        return dto;
    }

    public virtual async Task<TDto> PutAsync(TDto dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        entity.DateUpdated = DateTime.UtcNow;

        try
        {
            await dbRepository.Update(entity);
            await dbRepository.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!EntityExists(dto.Id))
                throw new EntityNotFoundException($"{CommonStrings.NotFoundResult}, {ex.Message}");
        }

        return dto;
    }

    private bool EntityExists(Guid id)
    {
        return dbRepository.Get<TEntity>(e => e.Id == id).Any();
    }
}

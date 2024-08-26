using AutoInterfaceAttributes;
using AutoMapper;
using Library.Common;
using Library.Core.DTO.Base;
using Library.Core.Models.Base;
using Library.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Services.Base;

[AutoInterface]
public class BaseService<TDto, TEntity>(DbContext dbContext, IMapper mapper) : IBaseService<TDto, TEntity>
    where TDto : BaseDto
    where TEntity : BaseModel
{
    
    protected readonly DbContext DbContext = dbContext;
    
    protected readonly IMapper Mapper = mapper;
    
    protected DbSet<TEntity> Context => DbContext.Set<TEntity>();
    
    public virtual async Task<TDto> GetByIdAsync(Guid id)
    {
        var entity = await Context.FindAsync(id);
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = Mapper.Map<TDto>(entity);
       
        return dto;
    }
    
    public virtual async Task DeleteByIdAsync(Guid id)
    {
        var entity = await Context.FindAsync(id);
        if (entity is null)
            throw new EntityNotFoundException($"{typeof(TEntity).Name} {CommonStrings.NotFoundResult}");

        Context.Remove(entity);
        await DbContext.SaveChangesAsync();
    }
    
    public virtual async Task<TDto> PostAsync(TDto dto)
    {
        await Context.AddAsync(Mapper.Map<TEntity>(dto));
        await DbContext.SaveChangesAsync();
        return dto;
    }
    public virtual async Task<TDto> PutAsync(Guid id, TDto dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        try
        {
            Context.Update(entity);
            await DbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!EntityExists(id))
                throw new EntityNotFoundException($"{CommonStrings.NotFoundResult}, {ex.Message}");
        }
        
        Context.Entry(entity).State = EntityState.Detached;
        
        return dto;
    }
    private bool EntityExists(Guid id)
    {
        return DbContext.Set<TEntity>().Any(e => e.Id == id);
    }
}
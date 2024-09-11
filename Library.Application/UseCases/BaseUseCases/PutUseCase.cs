namespace Library.Application.UseCases.BaseUseCases;

[AutoInterface]
public class PutUseCase<TDto, TEntity>(IDbRepository dbRepository, IMapper mapper) : IPutUseCase<TDto, TEntity>
    where TDto : BaseDto
    where TEntity : BaseModel
{
    protected readonly IMapper Mapper = mapper;
    
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
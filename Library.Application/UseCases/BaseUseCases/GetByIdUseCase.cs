namespace Library.Application.UseCases.BaseUseCases;

[AutoInterface]
public class GetByIdUseCase<TDto, TEntity>(IDbRepository dbRepository, IMapper mapper) : IGetByIdUseCase<TDto, TEntity>
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
}
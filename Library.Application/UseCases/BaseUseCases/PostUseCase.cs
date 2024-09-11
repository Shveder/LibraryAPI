namespace Library.Application.UseCases.BaseUseCases;

[AutoInterface]
public class PostUseCase<TDto, TEntity>(IDbRepository dbRepository, IMapper mapper) : IPostUseCase<TDto, TEntity>
    where TDto : BaseDto
    where TEntity : BaseModel
{
    protected readonly IMapper Mapper = mapper;
    
    public virtual async Task<TDto> PostAsync(TDto dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        await dbRepository.Add(entity);
        await dbRepository.SaveChangesAsync();
        
        return dto;
    }
}
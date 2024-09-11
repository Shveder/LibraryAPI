namespace Library.Application.UseCases.BaseUseCases;

[AutoInterface]
public class GetAllUseCase<TDto, TEntity>(IDbRepository dbRepository, IMapper mapper) : IGetAllUseCase<TDto, TEntity>
    where TDto : BaseDto
    where TEntity : BaseModel
{
    protected readonly IMapper Mapper = mapper;
    
    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var entities = await dbRepository.GetAll<TEntity>().ToListAsync();
        var dtos = Mapper.Map<IEnumerable<TDto>>(entities);
        
        return dtos;
    }
}
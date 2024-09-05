namespace Library.Infrastructure.Mappings;

public class BaseModelProfile<TEntity, TDto>: Profile
    where TEntity : BaseModel
    where TDto : BaseDto
{
    protected BaseModelProfile()
    {
        CreateMap<TDto, TEntity>(); 
        CreateMap<TEntity, TDto>();
    }
}
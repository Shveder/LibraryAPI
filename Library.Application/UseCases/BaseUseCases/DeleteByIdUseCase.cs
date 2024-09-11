namespace Library.Application.UseCases.BaseUseCases;

[AutoInterface]
public class DeleteByIdUseCase<TDto, TEntity>(IDbRepository dbRepository, IMapper mapper) : IDeleteByIdUseCase<TDto, TEntity>
     where TDto : BaseDto
     where TEntity : BaseModel
{
     protected readonly IMapper Mapper = mapper;
     
     public virtual async Task DeleteByIdAsync(Guid id)
     {
          var entity = await dbRepository.Get<TEntity>(e => e.Id == id).FirstOrDefaultAsync();
          if (entity is null)
               throw new EntityNotFoundException($"{typeof(TEntity).Name} {CommonStrings.NotFoundResult}");

          await dbRepository.Delete(entity);
          await dbRepository.SaveChangesAsync();
     }
}
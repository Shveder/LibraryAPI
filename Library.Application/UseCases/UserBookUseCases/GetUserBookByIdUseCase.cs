namespace Library.Application.UseCases.UserBookUseCases;

public class GetUserBookByIdUseCase(IDbRepository repository, IMapper mapper)
{
    public virtual async Task<UserBookDto> GetByIdAsync(Guid id)
    {
        var entity = await repository.Get<UserBook>(e => e.Id == id).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);

        var dto = mapper.Map<UserBookDto>(entity);
        
        return dto;
    }
}
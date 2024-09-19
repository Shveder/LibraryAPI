namespace Library.Application.UseCases.UserUseCases;

public class GetUserByIdUseCase(IDbRepository repository, IMapper mapper)
{
    public virtual async Task<UserDto> GetByIdAsync(Guid id)
    {
        var entity = await repository.Get<User>(e => e.Id == id).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);

        var dto = mapper.Map<UserDto>(entity);
        
        return dto;
    }
}
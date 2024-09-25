namespace Library.Application.UseCases.AuthorUseCases;

public class GetAuthorByIdUseCase(IDbRepository repository, IMapper mapper)
{
    public async Task<AuthorDto> GetByIdAsync(Guid id)
    {
        var entity = await repository.Get<Author>(e => e.Id == id).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);

        var dto = mapper.Map<AuthorDto>(entity);
        
        return dto;
    }
}
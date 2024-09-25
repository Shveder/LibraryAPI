namespace Library.Application.UseCases.AuthorUseCases;

public class GetAllAuthorsUseCase(IDbRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<AuthorDto>> GetAllAsync()
    {
        var entities = await repository.GetAll<Author>().ToListAsync();
        var dtos = mapper.Map<IEnumerable<AuthorDto>>(entities);
        
        return dtos;
    }
}
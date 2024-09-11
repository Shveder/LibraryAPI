using Library.Application.Exceptions;

namespace Library.Application.UseCases.AuthorUseCases;

[AutoInterface(Inheritance = [typeof(IPostUseCase<AuthorDto, Author>)])]
public class PostAuthorUseCase(IDbRepository repository, IMapper mapper)
    : PostUseCase<AuthorDto, Author>(repository, mapper), IPostAuthorUseCase
{
    private readonly IDbRepository _repository = repository;

    public override async Task<AuthorDto> PostAsync(AuthorDto dto)
    {
        if (await IsAuthorUnique(dto.Name))
            throw new IncorrectDataException("Author with this name already exists");
        
        var author = Mapper.Map<Author>(dto);
        
        await _repository.Add(author);
        await _repository.SaveChangesAsync();
        
        return dto;
    }
    
    private async Task<bool> IsAuthorUnique(string name)
    {
        var author = await _repository.Get<Author>(model => model.Name == name).FirstOrDefaultAsync();
        
        return author != null;
    }
}
namespace Library.Infrastructure.Services;

[AutoInterface(Inheritance = [typeof(IBaseService<AuthorDto, Author>)])]
public class AuthorService(DataContext dbContext, IMapper mapper, IDbRepository repository)
    : BaseService<AuthorDto, Author>(repository, mapper), IAuthorService
{
    private readonly IDbRepository _repository = repository;

    public override async Task<AuthorDto> PostAsync(AuthorDto dto)
    {
        if (await IsAuthorUnique(dto.Name))
            throw new IncorrectDataException("Author with this name already exists");
        
        var author = new Author()
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Birthday = dto.Birthday,
            Country = dto.Country
        };
        
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
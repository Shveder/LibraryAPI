namespace Library.Application.UseCases.AuthorUseCases;

public class PostAuthorUseCase(IDbRepository repository, IMapper mapper)
{
    public async Task<AuthorDto> PostAsync(AuthorDto dto)
    {
        if (await IsAuthorUnique(dto.Name))
            throw new IncorrectDataException("Author with this name already exists");
        
        var author = mapper.Map<Author>(dto);
        
        await repository.Add(author);
        await repository.SaveChangesAsync();
        
        return dto;
    }
    
    private async Task<bool> IsAuthorUnique(string name)
    {
        var author = await repository.Get<Author>(model => model.Name == name).FirstOrDefaultAsync();
        
        return author != null;
    }
}
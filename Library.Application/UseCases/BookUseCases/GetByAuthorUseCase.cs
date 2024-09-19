namespace Library.Application.UseCases.BookUseCases;

public class GetByAuthorUseCase(IDbRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<BookDto>> GetByAuthor(Guid authorId)
    {
        var author = await repository.Get<Author>(a => a.Id == authorId).FirstOrDefaultAsync();
        if (author is null)
            throw new IncorrectDataException("Author not found");
        
        var entities = await repository.Get<Book>(e => e.Author == author).
            Include(model => model.Author).ToListAsync();
        if (entities is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dtos = mapper.Map<IEnumerable<BookDto>>(entities);
       
        return dtos;
    }
}
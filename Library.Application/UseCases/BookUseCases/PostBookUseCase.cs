namespace Library.Application.UseCases.BookUseCases;

public class PostBookUseCase(IDbRepository repository, IMapper mapper)
{
    public async Task<BookDto> PostAsync(BookDto dto)
    {
        var author = await repository.Get<Author>(a => a.Id == dto.AuthorId).FirstOrDefaultAsync();
        if (author is null)
            throw new IncorrectDataException("Author not found");
        
        var book = mapper.Map<Book>(dto);
        book.Author = author;

        await repository.Add(book);
        await repository.SaveChangesAsync();
        
        return mapper.Map<BookDto>(book);
    }
}
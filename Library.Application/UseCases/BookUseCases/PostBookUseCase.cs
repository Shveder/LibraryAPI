namespace Library.Application.UseCases.BookUseCases;

[AutoInterface(Inheritance = [typeof(IPostUseCase<BookDto, Book>)])]
public class PostBookUseCase(IDbRepository repository, IMapper mapper)
    : PostUseCase<BookDto, Book>(repository, mapper), IPostBookUseCase
{
    private readonly IDbRepository _repository = repository;

    public override async Task<BookDto> PostAsync(BookDto dto)
    {
        var author = await _repository.Get<Author>(a => a.Id == dto.AuthorId).FirstOrDefaultAsync();
        if (author is null)
            throw new IncorrectDataException("Author not found");
        
        var book = Mapper.Map<Book>(dto);
        book.Author = author;

        await _repository.Add(book);
        await _repository.SaveChangesAsync();
        
        return Mapper.Map<BookDto>(book);
    }
}
namespace Library.Application.UseCases.UserBookUseCases;

[AutoInterface(Inheritance = [typeof(IPostUseCase<UserBookDto, UserBook>)])]
public class PostUserBookUseCase(IDbRepository repository, IMapper mapper)
    : PostUseCase<UserBookDto, UserBook>(repository, mapper), IPostUserBookUseCase
{
    private readonly IDbRepository _repository = repository;

    public override async Task<UserBookDto> PostAsync(UserBookDto dto)
    {
        var book = await _repository.Get<Book>(b => b.Id == dto.BookId).FirstOrDefaultAsync();
        if (book is null)
            throw new IncorrectDataException("Book not found");
        
        var user = await _repository.Get<User>(b => b.Id == dto.UserId).FirstOrDefaultAsync();
        if (user is null)
            throw new IncorrectDataException("User not found");
        
        var oldUserBook = await _repository.Get<UserBook>(model => model.Book == book).FirstOrDefaultAsync();
        if (oldUserBook is not null)
            throw new IncorrectDataException("Somebody already got this book");

        var userBook = Mapper.Map<UserBook>(dto);
        userBook.Book = book;
        userBook.User = user;
        book.IsAvailable = false;
        book.DateUpdated = DateTime.UtcNow;

        await _repository.Update(book);
        await _repository.Add(userBook);
        await _repository.SaveChangesAsync();

        return Mapper.Map<UserBookDto>(userBook); 
    }
}
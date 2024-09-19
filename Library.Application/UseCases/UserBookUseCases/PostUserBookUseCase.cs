namespace Library.Application.UseCases.UserBookUseCases;

public class PostUserBookUseCase(IDbRepository repository, IMapper mapper)
{
    public async Task<UserBookDto> PostAsync(UserBookDto dto)
    {
        var book = await repository.Get<Book>(b => b.Id == dto.BookId).FirstOrDefaultAsync();
        if (book is null)
            throw new IncorrectDataException("Book not found");
        
        var user = await repository.Get<User>(b => b.Id == dto.UserId).FirstOrDefaultAsync();
        if (user is null)
            throw new IncorrectDataException("User not found");
        
        var oldUserBook = await repository.Get<UserBook>(model => model.Book == book).FirstOrDefaultAsync();
        if (oldUserBook is not null)
            throw new IncorrectDataException("Somebody already got this book");

        var userBook = mapper.Map<UserBook>(dto);
        userBook.Book = book;
        userBook.User = user;
        book.IsAvailable = false;
        book.DateUpdated = DateTime.UtcNow;

        await repository.Update(book);
        await repository.Add(userBook);
        await repository.SaveChangesAsync();

        return mapper.Map<UserBookDto>(userBook); 
    }
}
namespace Library.Infrastructure.Services;

[AutoInterface(Inheritance = [typeof(IBaseService<UserBookDto, UserBook>)])]
public class UserBookService(DataContext dbContext, IMapper mapper, IDbRepository repository)
    : BaseService<UserBookDto, UserBook>(repository, mapper), IUserBookService
{
    private readonly IDbRepository _repository = repository;

    public override async Task<UserBookDto> PostAsync(UserBookDto dto)
    {
        var book = await _repository.Get<Book>(b => b.Id == dto.BookId).FirstOrDefaultAsync();
        if (book == null)
            throw new IncorrectDataException("Book not found");
        
        var user = await _repository.Get<User>(b => b.Id == dto.UserId).FirstOrDefaultAsync();
        if (user == null)
            throw new IncorrectDataException("User not found");
        
        var oldUserBook = await _repository.Get<UserBook>(model => model.Book == book).FirstOrDefaultAsync();
        if (oldUserBook != null)
            throw new IncorrectDataException("Somebody already got this book");

        var userBook = new UserBook
        {
            Book = book,
            User = user,
            DateReturn = dto.DateReturn,
            DateTaken = dto.DateTaken,
        };
        book.IsAvailable = false;
        book.DateUpdated = DateTime.UtcNow;
        
        await _repository.Update(book);
        await _repository.Add(userBook);
        await _repository.SaveChangesAsync();
        return dto;
    }
    
    public override async Task<IEnumerable<UserBookDto>> GetAllAsync()
    {
        var entities = await _repository.GetAll<UserBook>()
            .Include(model => model.User)
            .Include(model => model.Book).ToListAsync();
        var dtos = Mapper.Map<IEnumerable<UserBookDto>>(entities);
        return dtos;
    }
    public override async Task<UserBookDto> GetByIdAsync(Guid id)
    {
        var entity = await _repository.Get<UserBook>(e => e.Id == id)
            .Include(model => model.Book)
            .Include(model => model.User)
            .FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = Mapper.Map<UserBookDto>(entity);
       
        return dto;
    }

    public async Task<IEnumerable<UserBook>> GetBooksByUserId(Guid userId)
    {
        var user = await _repository.Get<User>(u => u.Id == userId).FirstOrDefaultAsync();
        if (user == null)
            throw new IncorrectDataException("User not found");
        
        var userBooks = await _repository.GetAll<UserBook>()
            .Where(ub => ub.User== user) 
            .Include(ub => ub.Book)
            .ToListAsync();
        
        if (!userBooks.Any())
            throw new IncorrectDataException("No books found for this user");
        
        return userBooks;
    }
    public override async Task DeleteByIdAsync(Guid id)
    {
        var entity = await repository.Get<UserBook>(e => e.Id == id)
            .Include(model=> model.Book)
            .FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException($"{nameof(UserBook)} {CommonStrings.NotFoundResult}");

        var book = await _repository.Get<Book>(b => b.Id == entity.Book.Id).FirstOrDefaultAsync();
        if (book is null)
            throw new EntityNotFoundException($"{nameof(UserBook)} {CommonStrings.NotFoundResult}");
        book.IsAvailable = true;
        book.DateUpdated = DateTime.UtcNow;
        
        await repository.Update(book);
        await repository.Delete<UserBook>(id);
        await repository.SaveChangesAsync();
    }
}

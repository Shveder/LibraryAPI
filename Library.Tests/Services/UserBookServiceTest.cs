namespace Library.Tests.Services;

[TestFixture]
public class UserBookServiceTest : BaseTest
{
    private IUserBookService _service;
    private Mock<IMapper> _mapperMock;

    [SetUp]
    public new void Setup()
    {
        base.Setup();

        _mapperMock = new Mock<IMapper>();

        _service = new UserBookService(
            Context,
            _mapperMock.Object,
            new DbRepository(Context));
    }

    [Test]
    public async Task GetUserBookById()
    {
        // Arrange
        var author = CreateAuthor(new Guid());
        var book = CreateBook(Guid.NewGuid(), author);
        var user = CreateUser(new Guid());
            
        await Context.Users.AddAsync(user);
        await Context.Authors.AddAsync(author);
        await Context.Books.AddAsync(book);
            
        var userBook = CreateUserBook(new Guid(), user, book);
            
        await Context.UserBooks.AddAsync(userBook);
        await Context.SaveChangesAsync();
            
        _mapperMock.Setup(m => m.Map<UserBookDto>(It.IsAny<UserBook>())).Returns(new UserBookDto
        {
            Id = userBook.Id
        });
            
        // Act
        var result = await _service.GetByIdAsync(userBook.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userBook.Id);
    }

    [Test]
    public async Task PostBook()
    {
        // Arrange
        var author = CreateAuthor(new Guid());
        var book = CreateBook(Guid.NewGuid(), author);
        var user = CreateUser(new Guid());
            
        await Context.Users.AddAsync(user);
        await Context.Authors.AddAsync(author);
        await Context.Books.AddAsync(book);
        await Context.SaveChangesAsync();
            
        var userBookDto = new UserBookDto()
        {
            Id = new Guid(),
            BookId = book.Id,
            UserId = user.Id,
            DateTaken = DateTime.UtcNow,
            DateReturn = DateTime.UtcNow.AddHours(1)
        };
            
        // Act
        var result = await _service.PostAsync(userBookDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userBookDto.Id);
    }


    [Test]
    public async Task GetAllUserBooks()
    { 
        // Arrange
        var author = CreateAuthor(new Guid());
        var book = CreateBook(Guid.NewGuid(), author);
        var user = CreateUser(new Guid());
            
        await Context.Users.AddAsync(user);
        await Context.Authors.AddAsync(author);
        await Context.Books.AddAsync(book);
            
        // Arrange
        var userBooks = new List<UserBook>
        {
            CreateUserBook(new Guid(), user, book),
            CreateUserBook(new Guid(), user, book)
        };

        await Context.UserBooks.AddRangeAsync(userBooks);
        await Context.SaveChangesAsync();
            
        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task PutUserBook()
    {
        // Arrange
        var author = CreateAuthor(new Guid());
        var book = CreateBook(Guid.NewGuid(), author);
        var user = CreateUser(new Guid());
            
        await Context.Users.AddAsync(user);
        await Context.Authors.AddAsync(author);
        await Context.Books.AddAsync(book);
            
        var userBook = CreateUserBook(new Guid(), user, book);

        await Context.UserBooks.AddAsync(userBook);
        await Context.SaveChangesAsync();

        var date = DateTime.UtcNow.AddHours(12);
        var updatedDto = new UserBookDto()
        {
            Id = userBook.Id,
            DateReturn = date
        };

        _mapperMock.Setup(m => m.Map<UserBook>(It.IsAny<UserBookDto>())).Returns(new UserBook
        {
            DateReturn = date
        });

        // Act
        var result = await _service.PutAsync(updatedDto);

        // Assert
        result.Should().NotBeNull();
        result.DateReturn.Should().Be(date);
    }

    [Test]
    public async Task DeleteUserBook()
    {
        // Arrange
        var author = CreateAuthor(Guid.NewGuid());
        var book = CreateBook(Guid.NewGuid(), author);
        var user = CreateUser(Guid.NewGuid());

        await Context.Users.AddAsync(user);
        await Context.Authors.AddAsync(author);
        await Context.Books.AddAsync(book);
        await Context.SaveChangesAsync();

        var userBook = CreateUserBook(new Guid(), user, book);

        await Context.UserBooks.AddAsync(userBook);
        await Context.SaveChangesAsync();

        // Act
        await _service.DeleteByIdAsync(userBook.Id);

        // Assert
        var deletedBook = await Context.Books.FindAsync(userBook.Id);
        deletedBook.Should().BeNull();
    }
        
    [Test]
    public async Task GetUserBooksByUser()
    {
        // Arrange
        var author = CreateAuthor(new Guid());
        var book = CreateBook(Guid.NewGuid(), author);
        var user = CreateUser(new Guid());
            
        await Context.Users.AddAsync(user);
        await Context.Authors.AddAsync(author);
        await Context.Books.AddAsync(book);


        var userBook = CreateUserBook(new Guid(), user, book);
        await Context.UserBooks.AddAsync(userBook);
        await Context.SaveChangesAsync();
            
        // Act
        var result = await _service.GetBooksByUserId(user.Id);

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(1);
    }

    private Author CreateAuthor(Guid authorId)
    {
        return new Author
        {
            Id = authorId,
            Name = "Author Name",
            Surname = "Author Surname",
            Country = "Author Country",
            Birthday = DateTime.UtcNow
        };
    }
    
    private Book CreateBook(Guid bookId, Author author)
    {
        return new Book()
        {
            Id = bookId,
            BookName = "Sample Book",
            Genre = "Sample Genre",
            Description = "Sample Description",
            ISBN = "123-456-789",
            IsAvailable = true,
            Author = author
        };
    }
    
    private User CreateUser(Guid userId)
    {
        return new User()
        {
            Id = userId,
            Login = "UserLogin",
            Password = "UserPassword",
            Salt = "UserSalt",
            Role = "User"
        };
    }
    
    private UserBook CreateUserBook(Guid userBookId, User user, Book book)
    {
        return new UserBook()
        {
            Id = new Guid(),
            Book = book,
            User = user,
            DateTaken = DateTime.UtcNow,
            DateReturn = DateTime.UtcNow.AddHours(1)
        };
    }
}
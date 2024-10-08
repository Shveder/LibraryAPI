﻿namespace Library.Tests.Services;

[TestFixture]
public class UserBookServiceTest : BaseTest
{
    private DeleteUserBookUseCase _deleteUserBook;
    private GetAllUserBooksUseCase _getAllUserBooks;
    private GetUserBookByIdUseCase _getUserBookById;
    private GetBookByUserUseCase _getBookByUser;
    private PostUserBookUseCase _postUserBook;
    private Mock<IMapper> _mapperMock;
    private DbRepository _repository;
    

    [SetUp]
    public new void Setup()
    {
        base.Setup();

        _mapperMock = new Mock<IMapper>();
        _repository = new DbRepository(Context);

        _deleteUserBook = new DeleteUserBookUseCase(_repository);
        _getAllUserBooks = new GetAllUserBooksUseCase(
            _repository,
            _mapperMock.Object);
        _getUserBookById = new GetUserBookByIdUseCase(
            _repository,
            _mapperMock.Object);
        _getBookByUser = new GetBookByUserUseCase(
            _repository);
        _postUserBook = new PostUserBookUseCase(
            _repository,
            _mapperMock.Object);
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
        var result = await _getUserBookById.GetByIdAsync(userBook.Id);

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
            Id = Guid.NewGuid(),
            BookId = book.Id,
            UserId = user.Id,
            DateTaken = DateTime.UtcNow,
            DateReturn = DateTime.UtcNow.AddHours(1)
        };

        var userBook = new UserBook
        {
            Id = userBookDto.Id,
            Book = book,
            User = user,
            DateTaken = userBookDto.DateTaken,
            DateReturn = userBookDto.DateReturn
        };

        _mapperMock.Setup(m => m.Map<UserBook>(userBookDto)).Returns(userBook);
        _mapperMock.Setup(m => m.Map<UserBookDto>(userBook)).Returns(userBookDto);

        // Act
        var result = await _postUserBook.PostAsync(userBookDto);

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
        var result = await _getAllUserBooks.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
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
        await _deleteUserBook.DeleteByIdAsync(userBook.Id);

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
        var result = await _getBookByUser.GetBooksByUserId(user.Id);

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
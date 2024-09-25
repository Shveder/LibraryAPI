namespace Library.Tests.Services;

[TestFixture]
public class BookUseCasesTests : BaseTest
{
    private GetBookByIdUseCase _getBookByIdUseCase;
    private PostBookUseCase _postBookUseCase;
    private GetAllBooksUseCase _getAllBooksUseCase;
    private PutBookUseCase _putBookUseCase;
    private DeleteBookUseCase _deleteBookUseCase;
    private GetBookByIsbnUseCase _getBookByIsbnUseCase;
    private GetByAuthorUseCase _getBooksByAuthorUseCase;
    private Mock<IMapper> _mapperMock;
    private DbRepository _repository;

    [SetUp]
    public new void Setup()
    {
        base.Setup();
        _mapperMock = new Mock<IMapper>();
        _repository = new DbRepository(Context);

        _getBookByIdUseCase = new GetBookByIdUseCase(_repository, _mapperMock.Object);
        _postBookUseCase = new PostBookUseCase(_repository, _mapperMock.Object);
        _getAllBooksUseCase = new GetAllBooksUseCase(_repository, _mapperMock.Object);
        _putBookUseCase = new PutBookUseCase(_repository, _mapperMock.Object);
        _deleteBookUseCase = new DeleteBookUseCase(_repository);
        _getBookByIsbnUseCase = new GetBookByIsbnUseCase(_repository, _mapperMock.Object);
        _getBooksByAuthorUseCase = new GetByAuthorUseCase(_repository, _mapperMock.Object);
    }

    [Test]
    public async Task GetBookById_ShouldReturnBook()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var author = CreateAuthor(Guid.NewGuid());
        var book = CreateBook(bookId, author);

        await Context.Books.AddAsync(book);
        await Context.SaveChangesAsync();

        _mapperMock.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(new BookDto
        {
            Id = bookId,
            Genre = "Sample Genre"
        });

        // Act
        var result = await _getBookByIdUseCase.GetByIdAsync(bookId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(bookId);
    }

    [Test]
    public async Task PostBook_ShouldAddBook()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var author = CreateAuthor(authorId);

        await Context.Authors.AddAsync(author);
        await Context.SaveChangesAsync();

        var bookDto = new BookDto
        {
            Id = bookId,
            BookName = "Sample Book",
            ISBN = "123-456-789",
            Genre = "Sample Genre",
            Description = "Sample Description",
            IsAvailable = true,
            AuthorId = authorId
        };

        var book = CreateBook(bookId, author);

        _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookDto>())).Returns(book);
        _mapperMock.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(bookDto);

        // Act
        await _postBookUseCase.PostAsync(bookDto);

        // Assert
        var addedBook = await Context.Books.FindAsync(bookId);
        addedBook.Should().NotBeNull();
        addedBook.BookName.Should().Be(bookDto.BookName);
    }

    [Test]
    public async Task GetAllBooks_ShouldReturnAllBooks()
    {
        // Arrange
        var author = CreateAuthor(Guid.NewGuid());
        var books = new List<Book>
        {
            CreateBook(Guid.NewGuid(), author),
            CreateBook(Guid.NewGuid(), author)
        };

        await Context.Books.AddRangeAsync(books);
        await Context.SaveChangesAsync();

        _mapperMock.Setup(m => m.Map<IEnumerable<BookDto>>(It.IsAny<List<Book>>()))
            .Returns(books.Select(b => new BookDto { Id = b.Id, Genre = b.Genre }).ToList());

        // Act
        var result = await _getAllBooksUseCase.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task PutBook_ShouldUpdateBook()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var author = CreateAuthor(Guid.NewGuid());
        var book = CreateBook(bookId, author);

        await Context.Books.AddAsync(book);
        await Context.SaveChangesAsync();

        var updatedDto = new BookDto
        {
            Id = bookId,
            BookName = "Updated Book",
            ISBN = "123-456-789"
        };

        _mapperMock.Setup(m => m.Map(It.IsAny<BookDto>(), It.IsAny<Book>())).Callback((BookDto dto, Book entity) =>
        {
            entity.BookName = dto.BookName;
            entity.ISBN = dto.ISBN;
            entity.DateUpdated = DateTime.UtcNow;
        });

        // Act
        var result = await _putBookUseCase.PutAsync(updatedDto);

        // Assert
        result.Should().NotBeNull();
        result.BookName.Should().Be("Updated Book");
    }

    [Test]
    public async Task DeleteBook_ShouldRemoveBook()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var author = CreateAuthor(Guid.NewGuid());
        var book = CreateBook(bookId, author);

        await Context.Books.AddAsync(book);
        await Context.SaveChangesAsync();

        // Act
        await _deleteBookUseCase.DeleteByIdAsync(bookId);
        var deletedBook = await Context.Books.FindAsync(bookId);

        // Assert
        deletedBook.Should().BeNull();
    }

    [Test]
    public async Task GetBookByIsbn_ShouldReturnBook()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var isbn = "123-456-789";
        var author = CreateAuthor(Guid.NewGuid());
        var book = CreateBook(bookId, author);

        await Context.Books.AddAsync(book);
        await Context.SaveChangesAsync();

        _mapperMock.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(new BookDto
        {
            Id = bookId,
            BookName = "Sample Book",
            ISBN = isbn
        });

        // Act
        var result = await _getBookByIsbnUseCase.GetByIsbnAsync(isbn);

        // Assert
        result.Should().NotBeNull();
        result.ISBN.Should().Be(isbn);
    }

    [Test]
    public async Task GetBooksByAuthor_ShouldReturnBooksByAuthor()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var author = CreateAuthor(authorId);
        var books = new List<Book>
        {
            CreateBook(Guid.NewGuid(), author),
            CreateBook(Guid.NewGuid(), author)
        };

        await Context.Books.AddRangeAsync(books);
        await Context.SaveChangesAsync();

        _mapperMock.Setup(m => m.Map<IEnumerable<BookDto>>(It.IsAny<List<Book>>()))
            .Returns(books.Select(b => new BookDto { Id = b.Id, BookName = b.BookName, AuthorId = authorId }).ToList());

        // Act
        var result = await _getBooksByAuthorUseCase.GetByAuthor(authorId);

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(books.Count);
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
        return new Book
        {
            Id = bookId,
            BookName = "Sample Book",
            ISBN = "123-456-789",
            Genre = "Sample Genre",
            Description = "Sample Description",
            IsAvailable = true,
            Author = author
        };
    }
}
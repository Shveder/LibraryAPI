namespace Library.Tests.Services
{
    [TestFixture]
    public class BookServiceTest : BaseTest
    {
        private IBookService _service;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public new void Setup()
        {
            base.Setup();

            _mapperMock = new Mock<IMapper>();

            _service = new BookService(
                Context,
                _mapperMock.Object,
                new DbRepository(Context));
        }

        [Test]
        public async Task GetBookById()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var author = CreateAuthor(new Guid());
            var book = CreateBook(bookId, author);

            await Context.Books.AddAsync(book);
            await Context.SaveChangesAsync();

            _mapperMock.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(new BookDto
            {
                Id = bookId,
                Genre = "Sample Genre"
            });

            // Act
            var result = await _service.GetByIdAsync(bookId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(bookId);
        }

        [Test]
        public async Task PostBook()
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
            var result = await _service.PostAsync(bookDto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(bookId);
        }

        [Test]
        public async Task GetAllBooks()
        { 
            var author = CreateAuthor(Guid.NewGuid());
            // Arrange
            var books = new List<Book>
            {
                CreateBook(new Guid(), author),
                CreateBook(new Guid(), author)
            };

            await Context.Books.AddRangeAsync(books);
            await Context.SaveChangesAsync();

            _mapperMock.Setup(m => m.Map<IEnumerable<BookDto>>(It.IsAny<List<Book>>()))
                .Returns(books.Select(b => new BookDto { Id = b.Id, Genre = b.Genre }).ToList());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task PutBook()
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

            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookDto>())).Returns(new Book
            {
                BookName = "Updated Book",
                ISBN = "123-456-789",
                DateUpdated = DateTime.UtcNow
            });

            // Act
            var result = await _service.PutAsync(updatedDto);

            // Assert
            result.Should().NotBeNull();
            result.BookName.Should().Be("Updated Book");
        }

        [Test]
        public async Task DeleteBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var author = CreateAuthor(Guid.NewGuid());
            var book = CreateBook(bookId, author);

            await Context.Books.AddAsync(book);
            await Context.SaveChangesAsync();

            // Act
            await _service.DeleteByIdAsync(bookId);
            var deletedBook = await Context.Books.FindAsync(bookId);

            // Assert
            deletedBook.Should().BeNull();
        }

        [Test]
        public async Task GetBookByIsbn()
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
            var result = await _service.GetByIsbnAsync(isbn);

            // Assert
            result.Should().NotBeNull();
            result.ISBN.Should().Be(isbn);
        }

        [Test]
        public async Task GetBooksByAuthor()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = CreateAuthor(authorId);
            var books = new List<Book>
            {
                CreateBook(new Guid(), author),
                CreateBook(new Guid(), author)
            };

            await Context.Books.AddRangeAsync(books);
            await Context.SaveChangesAsync();

            _mapperMock.Setup(m => m.Map<IEnumerable<BookDto>>(It.IsAny<List<Book>>()))
                .Returns(books.Select(b => new BookDto { Id = b.Id, BookName = b.BookName, AuthorId = authorId }).ToList());

            // Act
            var result = await _service.GetByAuthor(authorId);

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
            return new Book()
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
}

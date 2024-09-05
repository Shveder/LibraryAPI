namespace Library.Tests.Services;

[TestFixture]
public class AuthorServiceTest : BaseTest
{
    private IAuthorService _service;
    private Mock<IMapper> _mapperMock;

    [SetUp]
    public new void Setup()
    {
        base.Setup();
            
        _mapperMock = new Mock<IMapper>();
            
        _service = new AuthorService(
            _mapperMock.Object,
            new DbRepository(Context));
    }

    [Test]
    public async Task GetAuthorById()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var author = CreateAuthor(authorId);

        await Context.Authors.AddAsync(author);
        await Context.SaveChangesAsync();

        _mapperMock.Setup(m => m.Map<AuthorDto>(It.IsAny<Author>())).Returns(new AuthorDto
        {
            Id = authorId,
            Name = "John Doe"
        });

        // Act
        var result = await _service.GetByIdAsync(authorId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(authorId);
    }

    [Test]
    public async Task PostAuthor()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var author = CreateAuthor(authorId);

        // Act
        var result = await Context.Authors.AddAsync(author);
        await Context.SaveChangesAsync();

        _mapperMock.Setup(m => m.Map<AuthorDto>(It.IsAny<Author>())).Returns(new AuthorDto
        {
            Id = authorId,
            Name = "John Doe"
        });

        // Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task GetAllAuthors()
    {
        // Arrange
        var authors = new List<Author>
        {
            CreateAuthor(new Guid()),
            CreateAuthor(new Guid())
        };

        await Context.Authors.AddRangeAsync(authors);
        await Context.SaveChangesAsync();

        _mapperMock.Setup(m => m.Map<IEnumerable<AuthorDto>>(It.IsAny<List<Author>>()))
            .Returns(authors.Select(a => new AuthorDto { Id = a.Id, Name = a.Name }).ToList());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(authors.Count);
    }

    [Test]
    public async Task PutAuthor()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var author = CreateAuthor(authorId);

        await Context.Authors.AddAsync(author);
        await Context.SaveChangesAsync();

        var updatedDto = new AuthorDto
        {
            Id = authorId,
            Name = "John Updated"
        };

        _mapperMock.Setup(m => m.Map<Author>(It.IsAny<AuthorDto>())).Returns(new Author
        {
            Name = "John Updated",
            Surname = "Doe",
            Country = "Belarus",
            Birthday = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        });

        // Act
        var result = await _service.PutAsync(updatedDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Updated");
    }

    [Test]
    public async Task DeleteAuthor()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var author = CreateAuthor(authorId);
            
        await Context.Authors.AddAsync(author);
        await Context.SaveChangesAsync();

        // Act
        await _service.DeleteByIdAsync(authorId);
        var deletedAuthor = await Context.Authors.FindAsync(authorId);

        // Assert
        deletedAuthor.Should().BeNull();
    }

    public Author CreateAuthor(Guid authorId)
    {
        return new Author(){
            Id = authorId,
            Name = "John Doe",
            Surname = "Doe",
            Country = "Belarus",
            Birthday = DateTime.UtcNow
        };
    }
}
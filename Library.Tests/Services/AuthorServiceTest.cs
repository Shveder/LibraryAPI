namespace Library.Tests.Services;

[TestFixture]
public class AuthorUseCasesTests : BaseTest
{
    private GetAuthorByIdUseCase _getAuthorByIdUseCase;
    private PostAuthorUseCase _postAuthorUseCase;
    private GetAllAuthorsUseCase _getAllAuthorsUseCase;
    private DeleteAuthorByIdUseCase _deleteAuthorUseCase;
    private Mock<IMapper> _mapperMock;
    private DbRepository _repository;

    [SetUp]
    public new void Setup()
    {
        base.Setup();
        _mapperMock = new Mock<IMapper>();
        _repository = new DbRepository(Context);
        
        _getAuthorByIdUseCase = new GetAuthorByIdUseCase(_repository, _mapperMock.Object);
        _postAuthorUseCase = new PostAuthorUseCase(_repository, _mapperMock.Object);
        _getAllAuthorsUseCase = new GetAllAuthorsUseCase(_repository, _mapperMock.Object);
        _deleteAuthorUseCase = new DeleteAuthorByIdUseCase(_repository);
    }

    [Test]
    public async Task GetAuthorById_ShouldReturnAuthor()
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
        var result = await _getAuthorByIdUseCase.GetByIdAsync(authorId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(authorId);
    }

    [Test]
    public async Task PostAuthor_ShouldAddAuthor()
    {
        // Arrange
        var authorDto = new AuthorDto
        {
            Id = Guid.NewGuid(),
            Name = "John Doe"
        };

        var author = new Author
        {
            Id = authorDto.Id,
            Name = authorDto.Name,
            Surname = "Doe",
            Country = "Belarus",
            Birthday = DateTime.UtcNow
        };

        _mapperMock.Setup(m => m.Map<Author>(It.IsAny<AuthorDto>())).Returns(author);

        // Act
        await _postAuthorUseCase.PostAsync(authorDto);

        // Assert
        var addedAuthor = await Context.Authors.FindAsync(author.Id);
        addedAuthor.Should().NotBeNull();
        addedAuthor.Name.Should().Be(authorDto.Name);
    }

    [Test]
    public async Task GetAllAuthors_ShouldReturnAllAuthors()
    {
        // Arrange
        var authors = new List<Author>
        {
            CreateAuthor(Guid.NewGuid()),
            CreateAuthor(Guid.NewGuid())
        };

        await Context.Authors.AddRangeAsync(authors);
        await Context.SaveChangesAsync();

        _mapperMock.Setup(m => m.Map<IEnumerable<AuthorDto>>(It.IsAny<List<Author>>()))
            .Returns(authors.Select(a => new AuthorDto { Id = a.Id, Name = a.Name }).ToList());

        // Act
        var result = await _getAllAuthorsUseCase.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(authors.Count);
    }

    [Test]
    public async Task DeleteAuthor_ShouldRemoveAuthor()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var author = CreateAuthor(authorId);

        await Context.Authors.AddAsync(author);
        await Context.SaveChangesAsync();

        // Act
        await _deleteAuthorUseCase.DeleteByIdAsync(authorId);
        var deletedAuthor = await Context.Authors.FindAsync(authorId);

        // Assert
        deletedAuthor.Should().BeNull();
    }

    private Author CreateAuthor(Guid authorId)
    {
        return new Author
        {
            Id = authorId,
            Name = "John Doe",
            Surname = "Doe",
            Country = "Belarus",
            Birthday = DateTime.UtcNow
        };
    }
}
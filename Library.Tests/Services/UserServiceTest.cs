namespace Library.Tests.Services;

[TestFixture]
public class UserServiceTest : BaseTest
{
    private IGetUserByIdUseCase _getUserByIdUseCase;
    private Mock<IMapper> _mapperMock;

    [SetUp]
    public new void Setup()
    {
        base.Setup();

        _mapperMock = new Mock<IMapper>();

        _getUserByIdUseCase = new GetUserByIdUseCase(
            new DbRepository(Context),
            _mapperMock.Object);
    }

    [Test]
    public async Task GetUserById_ShouldReturnUserDto_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = CreateUser(userId);
            
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto
        {
            Id = userId,
            Login = "testuser",
            Role = "Admin"
        });

        // Act
        var result = await _getUserByIdUseCase.GetByIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userId);
        result.Login.Should().Be("testuser");
    }

    private User CreateUser(Guid userId)
    {
        return new User()
        {
            Id = userId,
            Login = "user",
            Password = "password",
            Salt = "salt",
            Role = "user"
        };
    }
}
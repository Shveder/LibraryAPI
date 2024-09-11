namespace Library.Tests.Services;

[TestFixture]
public class AuthorizationUseCasesTests : BaseTest
{
    private ILoginUseCase _loginUseCase;
    private IRegisterUseCase _registerUseCase;
    private Mock<IConfiguration> _configurationMock;
    private Mock<ILogger<RegisterUseCase>> _registerLoggerMock;
    private DbRepository _repository;
    private Mock<IMapper> _mapperMock;

    [SetUp]
    public new void Setup()
    {
        base.Setup();

        _configurationMock = new Mock<IConfiguration>();
        _registerLoggerMock = new Mock<ILogger<RegisterUseCase>>();
        _mapperMock = new Mock<IMapper>();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        });

        _repository = new DbRepository(Context);

        // Настройка маппера mock
        _mapperMock
            .Setup(m => m.Map<User>(It.IsAny<RegisterUserRequest>()))
            .Returns((RegisterUserRequest src) => new User
            {
                Login = src.Login,
                Password = "",
                Salt = "",
                Role = ""
            });

        _loginUseCase = new LoginUseCase(_configurationMock.Object, _repository);
        _registerUseCase = new RegisterUseCase(_mapperMock.Object, _repository, _registerLoggerMock.Object);
    }

    [Test]
    public async Task Register_ValidRequest_ShouldCreateUser()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Login = "new user",
            Password = "password",
            PasswordRepeat = "password"
        };

        // Act
        await _registerUseCase.Register(request);

        // Assert
        Assert.That(_repository.Get<User>(model => model.Login == request.Login), Is.Not.Null);
        await _repository.SaveChangesAsync();
    }

    [Test]
    public void Register_PasswordsDoNotMatch_ShouldThrowIncorrectDataException()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Login = "user",
            Password = "password1",
            PasswordRepeat = "password2"
        };

        // Act
        Func<Task> act = async () => await _registerUseCase.Register(request);

        // Assert
        act.Should().ThrowAsync<IncorrectDataException>().WithMessage("Passwords do not match");
    }

    [Test]
    public void Register_LoginAlreadyExists_ShouldThrowIncorrectDataException()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Login = "existing user",
            Password = "password",
            PasswordRepeat = "password"
        };
            
        // Act
        Func<Task> act = async () => await _registerUseCase.Register(request);

        // Assert
        act.Should().ThrowAsync<IncorrectDataException>().WithMessage("There is already a user with this login in the system");
    }

    [Test]
    public async Task GenerateTokenAsync_ValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var login = "tester";
        var password = "password";
        var salt = "salt";
        var hashedPassword = Hash(password);
        hashedPassword = Hash(hashedPassword + salt);
            
        var user = new User
        {
            Login = login,
            Password = hashedPassword,
            Salt = salt,
            Role = "Admin"
        };
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
            
        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("your_secret_key12345678901234567890");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("your_issuer");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("your_audience");

        // Act
        var token = await _loginUseCase.GenerateTokenAsync(login, password);

        // Assert
        token.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void GenerateTokenAsync_InvalidCredentials_ShouldThrowIncorrectDataException()
    {
        // Arrange
        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("your_secret_key");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("your_issuer");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("your_audience");

        // Act
        Func<Task> act = async () => await _loginUseCase.GenerateTokenAsync("invalid user", "invalid password");

        // Assert
        act.Should().ThrowAsync<IncorrectDataException>().WithMessage("Invalid login or password");
    }
    
    private string Hash(string inputString)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            
            return sb.ToString();
        }
    }
}
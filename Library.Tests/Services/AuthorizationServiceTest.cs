namespace Library.Tests.Services
{
    [TestFixture]
    public class AuthorizationServiceTests : BaseTest
    {
        private IAuthorizationService _service;
        private Mock<IConfiguration> _configurationMock;
        private Mock<ILogger<AuthorizationService>> _loggerMock;
        private DbRepository _repository;

        [SetUp]
        public new void Setup()
        {
            base.Setup();

            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<AuthorizationService>>();

            _repository = new DbRepository(Context);
            _service = new AuthorizationService(
                Context,
                _configurationMock.Object,
                _loggerMock.Object,
                _repository);
        }

        [Test]
        public void Login_InvalidCredentials_ShouldThrowIncorrectDataException()
        {
            // Arrange
            const string login = "invalid user";
            const string password = "invalid password";
            
            // Act
            Func<Task> act = async () => await _service.Login(login, password);

            // Assert
            act.Should().ThrowAsync<IncorrectDataException>().WithMessage("Invalid login or password");
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
            await _service.Register(request);

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
            Func<Task> act = async () => await _service.Register(request);

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
            Func<Task> act = async () => await _service.Register(request);

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
            var hashedPassword = _service.Hash(password);
            hashedPassword = _service.Hash(hashedPassword + salt);
            
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
            var token = await _service.GenerateTokenAsync(login, password);

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
            Func<Task> act = async () => await _service.GenerateTokenAsync("invalid user", "invalid password");

            // Assert
            act.Should().ThrowAsync<IncorrectDataException>().WithMessage("Invalid login or password");
        }
    }
}

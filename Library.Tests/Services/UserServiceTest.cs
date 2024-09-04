namespace Library.Tests.Services
{
    [TestFixture]
    public class UserServiceTest : BaseTest
    {
        private IUserService _service;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public new void Setup()
        {
            base.Setup();

            _mapperMock = new Mock<IMapper>();

            _service = new UserService(
                Context,
                _mapperMock.Object,
                new DbRepository(Context));
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
            var result = await _service.GetByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.Login.Should().Be("testuser");
        }

        [Test]
        public async Task PostUser_ShouldAddUserToDatabase()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDto = new UserDto
            {
                Id = userId,
                Login = "newuser",
                Password = "newpassword",
                Salt = "newsalt",
                Role = "User"
            };

            var user = CreateUser(userId);

            _mapperMock.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(user);
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

            // Act
            var result = await _service.PostAsync(userDto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.Login.Should().Be("newuser");

            var addedUser = await Context.Users.FindAsync(userId);
            addedUser.Should().NotBeNull();
        }

        [Test]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                CreateUser(new Guid()),
                CreateUser(new Guid())
            };

            await Context.Users.AddRangeAsync(users);
            await Context.SaveChangesAsync();

            _mapperMock.Setup(m => m.Map<IEnumerable<UserDto>>(It.IsAny<List<User>>()))
                .Returns(users.Select(u => new UserDto { Id = u.Id, Login = u.Login, Role = u.Role }).ToList());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(users.Count);
        }

        [Test]
        public async Task PutUser_ShouldUpdateUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = CreateUser(userId);

            await Context.Users.AddAsync(user);
            await Context.SaveChangesAsync();

            var updatedDto = new UserDto
            {
                Id = userId,
                Login = "updateduser",
                Password = "updatedpassword",
                Salt = "updatedsalt",
                Role = "Admin"
            };

            _mapperMock.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(new User
            {
                Login = "updateduser",
                Password = "updatedpassword",
                Salt = "updatedsalt",
                Role = "Admin",
                DateUpdated = DateTime.UtcNow
            });

            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(updatedDto);

            // Act
            var result = await _service.PutAsync(updatedDto);

            // Assert
            result.Should().NotBeNull();
            result.Login.Should().Be("updateduser");
            result.Role.Should().Be("Admin");
        }

        [Test]
        public async Task DeleteUser_ShouldRemoveUserFromDatabase()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = CreateUser(userId);

            await Context.Users.AddAsync(user);
            await Context.SaveChangesAsync();

            // Act
            await _service.DeleteByIdAsync(userId);
            var deletedUser = await Context.Users.FindAsync(userId);

            // Assert
            deletedUser.Should().BeNull();
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
}

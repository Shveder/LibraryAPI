namespace Library.Tests.Services
{
    [TestFixture]
    public class PhotoServiceTest
    {
        private PhotoService _service;
        private Mock<ILogger<PhotoService>> _loggerMock;
        private string _testRootLocation;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<PhotoService>>();
            _service = new PhotoService(_loggerMock.Object);

            // Set up test directory
            _testRootLocation = Path.Combine(Directory.GetCurrentDirectory(), "test_uploads");
            Directory.CreateDirectory(_testRootLocation);

            // Set private field _rootLocation to test directory using reflection
            typeof(PhotoService)
                .GetField("_rootLocation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_service, _testRootLocation);
        }

        [TearDown]
        public void Teardown()
        {
            // Clean up test directory
            if (Directory.Exists(_testRootLocation))
            {
                Directory.Delete(_testRootLocation, true);
            }
        }

        [Test]
        public async Task SavePhotoAsync_ShouldSaveFile_WhenInvoked()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var fileContent = Encoding.UTF8.GetBytes("test image content");
            using var fileStream = new MemoryStream(fileContent);

            // Act
            await _service.SavePhotoAsync(bookId, fileStream);

            // Assert
            var savedFilePath = Path.Combine(_testRootLocation, "books", bookId.ToString(), $"{bookId}.jpg");
            File.Exists(savedFilePath).Should().BeTrue();

            // Ensure the file stream is properly disposed before accessing the file
            await using var savedFileStream = new FileStream(savedFilePath, FileMode.Open, FileAccess.Read, FileShare.None);
            var savedContent = new byte[savedFileStream.Length];
            await savedFileStream.ReadAsync(savedContent, 0, savedContent.Length);
            savedContent.Should().BeEquivalentTo(fileContent);
        }

        [Test]
        public async Task GetPhotoAsync_ShouldReturnPhotoStream_WhenPhotoExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var content = Encoding.UTF8.GetBytes("photo content");
            var bookDir = Path.Combine(_testRootLocation, "books", bookId.ToString());
            Directory.CreateDirectory(bookDir);
            var filePath = Path.Combine(bookDir, $"{bookId}.jpg");
            await File.WriteAllBytesAsync(filePath, content);

            // Act
            await using var resultStream = await _service.GetPhotoAsync(bookId);

            // Assert
            resultStream.Should().NotBeNull();

            // Ensure the result stream is readable
            using var memoryStream = new MemoryStream();
            await resultStream.CopyToAsync(memoryStream);
            memoryStream.ToArray().Should().BeEquivalentTo(content);
        }
    }
}

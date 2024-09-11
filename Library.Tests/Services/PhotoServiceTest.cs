namespace Library.Tests.Services;

[TestFixture]
public class PhotoUseCasesTests
{
    private IAddPhotoUseCase _savePhotoUseCase;
    private IGetPhotoUseCase _getPhotoUseCase;
    private string _testRootLocation;

    [SetUp]
    public void Setup()
    {
        _testRootLocation = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(Path.Combine(_testRootLocation, "books"));

        _savePhotoUseCase = new AddPhotoUseCase();
        _getPhotoUseCase = new GetPhotoUseCase();
        
        typeof(AddPhotoUseCase)
            .GetField("_rootLocation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_savePhotoUseCase, _testRootLocation);
    }

    [TearDown]
    public void Teardown()
    {
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
        await _savePhotoUseCase.SavePhotoAsync(bookId, fileStream);

        // Assert
        var savedFilePath = Path.Combine(_testRootLocation, "books", bookId.ToString(), $"{bookId}.jpg");
        File.Exists(savedFilePath).Should().BeTrue();

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
        await using var resultStream = await _getPhotoUseCase.GetPhotoAsync(bookId);

        // Assert
        resultStream.Should().NotBeNull();

        // Ensure the result stream is readable
        using var memoryStream = new MemoryStream();
        await resultStream.CopyToAsync(memoryStream);
        memoryStream.ToArray().Should().BeEquivalentTo(content);
    }
}
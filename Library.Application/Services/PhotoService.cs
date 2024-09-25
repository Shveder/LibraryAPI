namespace Library.Application.Services;

[AutoInterface]
public class PhotoService : IPhotoService
{
    private const string RootLocation = "uploads";

    public async Task SavePhotoAsync(Guid bookId, Stream fileStream)
    {
        var location = $"books/{bookId}";
        var bookDir = Path.Combine(RootLocation, location);
        Directory.CreateDirectory(bookDir);
        var filePath = Path.Combine(bookDir, $"{bookId}.jpg");

        if (File.Exists(filePath))
            File.Delete(filePath);

        await using var file = File.Create(filePath);
        await fileStream.CopyToAsync(file);
    }
    
    public async Task<Stream> GetPhotoAsync(Guid bookId)
    {
        var location = $"books/{bookId}";
        var filePath = Path.Combine(RootLocation, location, $"{bookId}.jpg");

        if (!File.Exists(filePath))
            filePath = Path.Combine(RootLocation, "profileIcon.png");
            
        return await Task.FromResult(File.OpenRead(filePath));
    }
}
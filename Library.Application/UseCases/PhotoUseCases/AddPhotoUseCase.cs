namespace Library.Application.UseCases.PhotoUseCases;

[AutoInterface]
public class AddPhotoUseCase : IAddPhotoUseCase
{
    private readonly string _rootLocation = "uploads";

    public async Task SavePhotoAsync(Guid bookId, Stream fileStream)
    {
        string location = $"books/{bookId}";
        string bookDir = Path.Combine(_rootLocation, location);
        Directory.CreateDirectory(bookDir);
        string filePath = Path.Combine(bookDir, $"{bookId}.jpg");

        if (File.Exists(filePath))
            File.Delete(filePath);

        await using var file = File.Create(filePath);
        await fileStream.CopyToAsync(file);
    }
}
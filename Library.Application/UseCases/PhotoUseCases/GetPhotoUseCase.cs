namespace Library.Application.UseCases.PhotoUseCases;

[AutoInterface]
public class GetPhotoUseCase : IGetPhotoUseCase
{
    private readonly string _rootLocation = "uploads";
    
    public async Task<Stream> GetPhotoAsync(Guid bookId)
    {
        string location = $"books/{bookId}";
        string filePath = Path.Combine(_rootLocation, location, $"{bookId}.jpg");

        if (!File.Exists(filePath))
            filePath = Path.Combine(_rootLocation, "profileIcon.png");
            
        return await Task.FromResult(File.OpenRead(filePath));
    }
}
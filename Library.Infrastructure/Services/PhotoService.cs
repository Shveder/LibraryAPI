using AutoInterfaceAttributes;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Services;

[AutoInterface]
public class PhotoService(ILogger<PhotoService> logger) : IPhotoService
{
    private readonly string _rootLocation = "uploads";

        public async Task SavePhotoAsync(Guid bookId, Stream fileStream)
        {
            try
            {
                string location =  $"books/{bookId}";
                string bookDir = Path.Combine(_rootLocation, location);
                Directory.CreateDirectory(bookDir);

                string filePath = Path.Combine(bookDir, $"{bookId}.jpg");
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                
                using (var file = File.Create(filePath))
                {
                    await fileStream.CopyToAsync(file);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Cant save photo for book {bookId}: {ex.Message}");
                throw;
            }
        }

        public async Task<Stream> GetPhotoAsync(Guid bookId)
        {
            try
            {
                string location = $"books/{bookId}";
                string filePath = Path.Combine(_rootLocation, location, $"{bookId}.jpg");

                if (!File.Exists(filePath))
                {
                    filePath = Path.Combine(_rootLocation, "profileIcon.png");
                }
                
                return await Task.FromResult(File.OpenRead(filePath));
            }
            catch (Exception ex)
            {
                logger.LogError($"Cant get photo for book {bookId}: {ex.Message}");
                throw;
            }
        }
}
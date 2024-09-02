namespace Library.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PhotoController(IPhotoService photoService)
    : ControllerBase
{
    [HttpPost("HandleFileUpload/{bookId}")]
    public async Task<IActionResult> HandleFileUpload([FromRoute] Guid bookId, IFormFile file)
    {
        await photoService.SavePhotoAsync(bookId, file.OpenReadStream());
        
        return Ok("Uploaded successfully");
    }

    [HttpGet("GetPhoto/{bookId}")]
    public async Task<IActionResult> GetPhoto([FromRoute] Guid bookId)
    {
        var photoStream = await photoService.GetPhotoAsync(bookId);
        
        return File(photoStream, "image/jpeg");
    }
}
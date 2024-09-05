namespace Library.API.Controllers;

/// <summary>
/// Controller responsible for handling photo uploads and retrievals related to books.
/// </summary>
/// <param name="photoService">Service that manages photo storage and retrieval for books.</param>
[ApiController]
[Route("[controller]")]
public class PhotoController(IPhotoService photoService) : ControllerBase
{
    /// <summary>
    /// Handles the file upload for a specific book.
    /// </summary>
    /// <param name="bookId">The unique identifier of the book associated with the photo.</param>
    /// <param name="file">The photo file to be uploaded.</param>
    /// <returns>
    /// A confirmation message indicating that the photo was uploaded successfully.
    /// </returns>
    [HttpPost("HandleFileUpload/{bookId}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> HandleFileUpload([FromRoute] Guid bookId, IFormFile file)
    {
        await photoService.SavePhotoAsync(bookId, file.OpenReadStream());
        
        return Ok("Uploaded successfully");
    }

    /// <summary>
    /// Retrieves the photo associated with a specific book.
    /// </summary>
    /// <param name="bookId">The unique identifier of the book whose photo is being retrieved.</param>
    /// <returns>
    /// The photo stream of the requested book in JPEG format.
    /// </returns>
    [HttpGet("GetPhoto/{bookId}")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPhoto([FromRoute] Guid bookId)
    {
        var photoStream = await photoService.GetPhotoAsync(bookId);
        
        return File(photoStream, "image/jpeg");
    }
}
namespace Library.API.Controllers;

/// <summary>
/// Controller responsible for handling photo uploads and retrievals related to books.
/// </summary>
///  <param name="getPhotoUseCase">Use case to get photo from folder.</param>
///  <param name="addPhotoUseCase">Use case to add photo to folder.</param>
[ApiController]
[Route("[controller]")]
public class PhotoController(IAddPhotoUseCase addPhotoUseCase, IGetPhotoUseCase getPhotoUseCase) : ControllerBase
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
        await addPhotoUseCase.SavePhotoAsync(bookId, file.OpenReadStream());
        
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
        var photoStream = await getPhotoUseCase.GetPhotoAsync(bookId);
        
        return File(photoStream, "image/jpeg");
    }
}
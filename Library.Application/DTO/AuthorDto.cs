namespace Library.Application.DTO;

public class AuthorDto : BaseDto
{
    /// <summary>
    /// Name of author
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Surname of author
    /// </summary>
    public string Surname { get; set; }
    
    /// <summary>
    /// Birthday of author
    /// </summary>
    public DateTime Birthday { get; set; }
    
    /// <summary>
    /// Country of author
    /// </summary>
    public string Country { get; set; }
}
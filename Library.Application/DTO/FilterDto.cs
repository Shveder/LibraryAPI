namespace Library.Application.DTO;

/// <summary>
/// Сlass for data filtering.
/// </summary>
public class FilterDto
{
    /// <summary>
    /// Page number for pagination. Default value: 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Page size for pagination. Default value: 10.
    /// </summary>
    public int PageSize { get; set; } = 10;
    
    /// <summary>
    /// Property name to sort by.
    /// </summary>
    public Guid? AuthorId { get; set; }
    
    /// <summary>
    /// Property name to sort by.
    /// </summary>
    public string? Genre { get; set; }
    
    /// <summary>
    /// Search input of user.
    /// </summary>
    public string? Search { get; set; }
}
using Library.Core.Models.Base;

namespace Library.Core.Models;

public class Author : BaseModel
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
    /// Author's birthday
    /// </summary>
    public DateTime Birthday { get; set; }
    /// <summary>
    /// Author's country
    /// </summary>
    public string Country { get; set; }
}
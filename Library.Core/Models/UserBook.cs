using Library.Core.Models.Base;

namespace Library.Core.Models;

public class UserBook : BaseModel
{
    /// <summary>
    /// User who took the book
    /// </summary>
    public User User { get; set; }
    
    /// <summary>
    /// Book that was taken
    /// </summary>
    public Book Book { get; set; }
    
    /// <summary>
    /// Date when book was taken
    /// </summary>
    public DateTime DateTaken { get; set; }
    
    /// <summary>
    /// The date the book is due to be returned
    /// </summary>
    public DateTime DateReturn { get; set; }
}
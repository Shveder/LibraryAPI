namespace Library.Application.DTO;

public class UserBookDto : BaseDto
{
    /// <summary>
    /// Id of user, who takes the book
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// The book that was taken
    /// </summary>
    public Guid BookId { get; set; }
    
    /// <summary>
    /// Time when book was taken
    /// </summary>
    public DateTime DateTaken { get; set; }
    
    /// <summary>
    /// Date the book needs to be returned
    /// </summary>
    public DateTime DateReturn { get; set; }
}
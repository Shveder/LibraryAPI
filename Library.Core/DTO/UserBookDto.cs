using Library.Core.DTO.Base;

namespace Library.Core.DTO;

public class UserBookDto : BaseDto
{
    public Guid UserId { get; set; }
    
    public Guid BookId { get; set; }
    
    public DateTime DateTaken { get; set; }
    
    public DateTime DateReturn { get; set; }
}
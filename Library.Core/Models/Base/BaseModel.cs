using Library.Core.Models.Interfaces;

namespace Library.Core.Models.Base;

public class BaseModel : IHasId
{
    /// <summary>
    /// ID of entity
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Date when model was created
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Date when model was updated
    /// </summary>
    public DateTime? DateUpdated { get; set; }
}
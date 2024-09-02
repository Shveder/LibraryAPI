using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Core.Models.Interfaces;

namespace Library.Core.Models.Base;

public class BaseModel : IHasId
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public DateTime? DateUpdated { get; set; }
}
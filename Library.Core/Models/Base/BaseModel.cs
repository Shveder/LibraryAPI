using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models.Base;

public class BaseModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateUpdated { get; set; }
}
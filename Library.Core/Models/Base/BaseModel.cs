using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models.Base;

public class BaseModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
}
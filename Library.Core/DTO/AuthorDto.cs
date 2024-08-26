using Library.Core.DTO.Base;

namespace Library.Core.DTO;

public class AuthorDto : BaseDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime Birthday { get; set; }
    public string Country { get; set; }
}
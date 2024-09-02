namespace Library.Infrastructure.Mappings;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    { 
        CreateMap<AuthorDto, Author>(); 
        CreateMap<Author, AuthorDto>();
    }
}
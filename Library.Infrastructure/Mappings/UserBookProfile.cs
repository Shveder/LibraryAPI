namespace Library.Infrastructure.Mappings;

public class UserBookProfile : Profile
{
    public UserBookProfile()
    { 
        CreateMap<UserBookDto, UserBook>(); 
        CreateMap<UserBook, UserBookDto>();
    }
}
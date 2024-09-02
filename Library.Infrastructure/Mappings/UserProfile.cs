namespace Library.Infrastructure.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    { 
        CreateMap<UserDto, User>(); 
        CreateMap<User, UserDto>();
    }
}
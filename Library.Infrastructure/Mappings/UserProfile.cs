namespace Library.Infrastructure.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, User>(); 
        CreateMap<User, UserDto>();
        
        CreateMap<RegisterUserRequest, User>()
            .ForMember(dest => dest.Salt, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }
}
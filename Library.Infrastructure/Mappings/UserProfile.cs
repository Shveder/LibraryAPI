using AutoMapper;
using Library.Core.DTO;
using Library.Core.Models;

namespace Library.Infrastructure.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    { 
        CreateMap<UserDto, User>(); 
        CreateMap<User, UserDto>();
    }
}
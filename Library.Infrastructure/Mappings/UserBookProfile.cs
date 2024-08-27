using AutoMapper;
using Library.Core.DTO;
using Library.Core.Models;

namespace Library.Infrastructure.Mappings;

public class UserBookProfile : Profile
{
    public UserBookProfile()
    { 
        CreateMap<UserBookDto, UserBook>(); 
        CreateMap<UserBook, UserBookDto>();
    }
}
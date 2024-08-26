using AutoMapper;
using Library.Core.DTO;
using Library.Core.Models;

namespace Library.Infrastructure.Mappings;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    { 
        CreateMap<AuthorDto, Author>(); 
        CreateMap<Author, AuthorDto>();
    }
}
using AutoMapper;
using Library.Core.DTO;
using Library.Core.Models;

namespace Library.Infrastructure.Mappings;

public class BookProfile : Profile
{
 public BookProfile()
    { 
        CreateMap<BookDto, Book>(); 
        CreateMap<Book, BookDto>();
    }
}
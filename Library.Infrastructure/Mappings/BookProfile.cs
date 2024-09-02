namespace Library.Infrastructure.Mappings;

public class BookProfile : Profile
{
 public BookProfile()
    { 
        CreateMap<BookDto, Book>(); 
        CreateMap<Book, BookDto>();
    }
}
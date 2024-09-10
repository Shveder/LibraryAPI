namespace Library.Infrastructure.Mappings;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<BookDto, Book>()
            .ForMember(dest => dest.Author, opt => opt.Ignore());
        
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id));
    }
}
namespace Library.Application.UseCases.BookUseCases;

public class GetAllBooksUseCase(IDbRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var entities = repository.GetAll<Book>().Include(b => b.Author).AsQueryable();
        var dtos = mapper.Map<IEnumerable<BookDto>>(entities);
        
        return dtos;
    }
}
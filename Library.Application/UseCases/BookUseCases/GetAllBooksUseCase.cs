namespace Library.Application.UseCases.BookUseCases;

[AutoInterface(Inheritance = [typeof(IGetAllUseCase<BookDto, Book>)])]
public class GetAllBooksUseCase(IDbRepository repository, IMapper mapper)
    : GetAllUseCase<BookDto, Book>(repository, mapper), IGetAllBooksUseCase
{
    private readonly IDbRepository _repository = repository;

    public override async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var entities = _repository.GetAll<Book>().Include(b => b.Author).AsQueryable();
        var dtos = Mapper.Map<IEnumerable<BookDto>>(entities);
        
        return dtos;
    }
}
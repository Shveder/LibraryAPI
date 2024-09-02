namespace Library.Infrastructure.Services;

[AutoInterface(Inheritance = [typeof(IBaseService<BookDto, Book>)])]
public class BookService(DataContext dbContext, IMapper mapper, IDbRepository repository)
    : BaseService<BookDto, Book>(repository, mapper), IBookService
{
    private readonly IDbRepository _repository = repository;

    public override async Task<BookDto> PostAsync(BookDto dto)
    {
        var author = await _repository.Get<Author>(a => a.Id == dto.AuthorId).FirstOrDefaultAsync();
        if (author is null)
            throw new IncorrectDataException("Author not found");
        
        var book = Mapper.Map<Book>(dto);
        
        await _repository.Add(book);
        await _repository.SaveChangesAsync();
        
        return dto;
    }
    
    public override async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var entities = _repository.GetAll<Book>().Include(b => b.Author);
        var dtos = Mapper.Map<IEnumerable<BookDto>>(entities);
        
        return dtos;
    }
    
    public override async Task<BookDto> GetByIdAsync(Guid id)
    {
        var entity = await _repository.Get<Book>(e => e.Id == id)
            .Include(model => model.Author).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = Mapper.Map<BookDto>(entity);
       
        return dto;
    }
    
    public async Task<BookDto> GetByIsbnAsync(string isbn)
    {
        var entity = await _repository.Get<Book>(e => e.ISBN == isbn)
            .Include(model => model.Author).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = Mapper.Map<BookDto>(entity);
       
        return dto;
    }
    
    public async Task<IEnumerable<BookDto>> GetByAuthor(Guid authorId)
    {
        var author = await _repository.Get<Author>(a => a.Id == authorId).FirstOrDefaultAsync();
        if (author is null)
            throw new IncorrectDataException("Author not found");
        
        var entities = await _repository.Get<Book>(e => e.Author == author).
                Include(model => model.Author).ToListAsync();
        if (entities is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dtos = Mapper.Map<IEnumerable<BookDto>>(entities);
       
        return dtos;
    }
}
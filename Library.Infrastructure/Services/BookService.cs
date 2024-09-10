using System.ComponentModel;
using System.Linq.Expressions;

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
        
        var book = mapper.Map<Book>(dto);
        book.Author = author;

        await _repository.Add(book);
        await _repository.SaveChangesAsync();
        
        return mapper.Map<BookDto>(book);
    }
    
    public override async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var entities = _repository.GetAll<Book>().Include(b => b.Author).AsQueryable();
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
    
    public virtual async Task<(IEnumerable<BookDto> Books, int TotalCount)> GetAllFiltered(FilterDto filter)
    {
        IQueryable<Book> query = _repository.GetAll<Book>().Include(b => b.Author).AsQueryable();

        if (filter.AuthorId.HasValue)
            query = query.Where(b => b.Author.Id == filter.AuthorId.Value);
        
        if (!string.IsNullOrEmpty(filter.Genre))
            query = query.Where(b => EF.Functions.Like(b.Genre, filter.Genre));
        
        if (!string.IsNullOrEmpty(filter.Search))
        {
            var searchPattern = $"%{filter.Search.ToLower()}%";
            query = query.Where(b => EF.Functions.Like(b.BookName.ToLower(), searchPattern) 
                                     || EF.Functions.Like(b.Description.ToLower(), searchPattern));
        }
       
        var totalCount = await query.CountAsync();

        var skip = (filter.PageNumber - 1) * filter.PageSize;
        var books = await query.Skip(skip).Take(filter.PageSize).ToListAsync();

        var bookDtos = books.Select(entity => Mapper.Map<BookDto>(entity)).ToList();

        return (bookDtos, totalCount);
    }
}
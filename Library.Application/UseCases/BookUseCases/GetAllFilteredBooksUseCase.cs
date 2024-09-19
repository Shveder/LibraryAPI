namespace Library.Application.UseCases.BookUseCases;

public class GetAllFilteredBooksUseCase(IDbRepository repository, IMapper mapper)
{
    public virtual async Task<(IEnumerable<BookDto> Books, int TotalCount)> GetAllFiltered(FilterDto filter)
    {
        IQueryable<Book> query = repository.GetAll<Book>().Include(b => b.Author).AsQueryable();

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

        var bookDtos = books.Select(entity => mapper.Map<BookDto>(entity)).ToList();

        return (bookDtos, totalCount);
    }
}
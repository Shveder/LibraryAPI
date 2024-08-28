using AutoInterfaceAttributes;
using AutoMapper;
using Library.Common;
using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.DatabaseContext;
using Library.Infrastructure.Exceptions;
using Library.Infrastructure.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Services;

[AutoInterface(Inheritance = [typeof(IBaseService<BookDto, Book>)])]
public class BookService(DataContext dbContext, IMapper mapper)
    : BaseService<BookDto, Book>(dbContext, mapper), IBookService
{

    public override async Task<BookDto> PostAsync(BookDto dto)
    {
        var author = await DbContext.Set<Author>().FindAsync(dto.AuthorId);
        if (author == null)
        {
            throw new IncorrectDataException("Author not found");
        }

        var book = new Book()
        {
            BookName = dto.BookName,
            ISBN = dto.ISBN,
            Author = author,
            Description = dto.Description,
            Genre = dto.Genre
        };
        
        await Context.AddAsync(book);
        await DbContext.SaveChangesAsync();
        return dto;
    }
    
    public override async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var entities = await Context.Include(model => model.Author).ToListAsync();
        var dtos = Mapper.Map<IEnumerable<BookDto>>(entities);
        return dtos;
    }
    
    public override async Task<BookDto> GetByIdAsync(Guid id)
    {
        var entity = await DbContext.Set<Book>()
            .Include(model => model.Author)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = Mapper.Map<BookDto>(entity);
       
        return dto;
    }
    public async Task<BookDto> GetByIsbnAsync(string isbn)
    {
        var entity = await DbContext.Set<Book>()
            .Include(model => model.Author)
            .FirstOrDefaultAsync(e => e.ISBN == isbn);
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = Mapper.Map<BookDto>(entity);
       
        return dto;
    }
    public async Task<BookDto> GetByAuthor(Guid authorId)
    {
        var author = await DbContext.Set<Author>().FindAsync(authorId);
        if (author == null)
            throw new IncorrectDataException("Author not found");
        
        var entity = await DbContext.Set<Book>()
            .Include(model => model.Author)
            .FirstOrDefaultAsync(e => e.Author == author);
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = Mapper.Map<BookDto>(entity);
       
        return dto;
    }
}
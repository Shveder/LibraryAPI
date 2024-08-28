using AutoInterfaceAttributes;
using AutoMapper;
using Library.Common;
using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.DatabaseContext;
using Library.Infrastructure.Exceptions;
using Library.Infrastructure.Repository;
using Library.Infrastructure.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Services;

[AutoInterface(Inheritance = [typeof(IBaseService<BookDto, Book>)])]
public class BookService(DataContext dbContext, IMapper mapper, IDbRepository repository)
    : BaseService<BookDto, Book>(repository, mapper), IBookService
{
    private readonly IDbRepository _repository = repository;

    public override async Task<BookDto> PostAsync(BookDto dto)
    {
        var author = await repository.Get<Author>(a => a.Id == dto.AuthorId).FirstOrDefaultAsync();
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
        
        await repository.Add(book);
        await repository.SaveChangesAsync();
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
    public async Task<BookDto> GetByAuthor(Guid authorId)
    {
        var author = await _repository.Get<Author>(a => a.Id == authorId).FirstOrDefaultAsync();
        if (author == null)
            throw new IncorrectDataException("Author not found");
        
        var entity = await _repository.Get<Book>(e => e.Author == author).
                Include(model => model.Author).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = Mapper.Map<BookDto>(entity);
       
        return dto;
    }
}
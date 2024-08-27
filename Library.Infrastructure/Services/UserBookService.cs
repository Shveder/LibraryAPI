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

[AutoInterface(Inheritance = [typeof(IBaseService<UserBookDto, UserBook>)])]
public class UserBookService(DataContext dbContext, IMapper mapper)
    : BaseService<UserBookDto, UserBook>(dbContext, mapper), IUserBookService
{
    public override async Task<UserBookDto> PostAsync(UserBookDto dto)
    {
        var book = await DbContext.Set<Book>().FindAsync(dto.BookId);
        if (book == null)
            throw new IncorrectDataException("Book not found");
        
        var user = await DbContext.Set<User>().FindAsync(dto.UserId);
        if (user == null)
            throw new IncorrectDataException("User not found");
        
        var oldUserBook = await DbContext.Set<UserBook>().FirstOrDefaultAsync(model => model.Book == book);
        if (oldUserBook != null)
            throw new IncorrectDataException("Somebody already got this book");

        var userBook = new UserBook
        {
            Book = book,
            User = user,
            DateReturn = dto.DateReturn,
            DateTaken = dto.DateTaken,
        };
        
        await Context.AddAsync(userBook);
        await DbContext.SaveChangesAsync();
        return dto;
    }
    
    public override async Task<IEnumerable<UserBookDto>> GetAllAsync()
    {
        var entities = await Context.Include(model => model.User)
            .Include(model => model.Book).ToListAsync();
        var dtos = Mapper.Map<IEnumerable<UserBookDto>>(entities);
        return dtos;
    }
    public override async Task<UserBookDto> GetByIdAsync(Guid id)
    {
        var entity = await DbContext.Set<UserBook>()
            .Include(model => model.Book)
            .Include(model => model.User)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = Mapper.Map<UserBookDto>(entity);
       
        return dto;
    }
}

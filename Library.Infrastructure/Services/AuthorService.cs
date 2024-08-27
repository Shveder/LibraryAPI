using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.DatabaseContext;
using Library.Infrastructure.Exceptions;
using Library.Infrastructure.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Services;

using AutoInterfaceAttributes;
using AutoMapper;


[AutoInterface(Inheritance = [typeof(IBaseService<AuthorDto, Author>)])]
public class AuthorService(DataContext dbContext, IMapper mapper)
    : BaseService<AuthorDto, Author>(dbContext, mapper), IAuthorService
{
    public override async Task<AuthorDto> PostAsync(AuthorDto dto)
    {
        if (await IsAuthorUnique(dto.Name))
            throw new IncorrectDataException("Author with this name already exists");
        
        var author = new Author()
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Birthday = dto.Birthday,
            Country = dto.Country
        };
        
        await Context.AddAsync(author);
        await DbContext.SaveChangesAsync();
        return dto;
    }
    private async Task<bool> IsAuthorUnique(string name)
    {
        var author = await DbContext.Set<Author>().FirstOrDefaultAsync(model => model.Name == name); 
        return author != null;
    }
}
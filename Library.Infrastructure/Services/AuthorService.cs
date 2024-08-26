using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.DatabaseContext;
using Library.Infrastructure.Services.Base;

namespace Library.Infrastructure.Services;

using AutoInterfaceAttributes;
using AutoMapper;


[AutoInterface(Inheritance = [typeof(IBaseService<AuthorDto, Author>)])]
public class AuthorService(DataContext dbContext, IMapper mapper)
    : BaseService<AuthorDto, Author>(dbContext, mapper), IAuthorService;
using AutoInterfaceAttributes;
using AutoMapper;
using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.DatabaseContext;
using Library.Infrastructure.Services.Base;

namespace Library.Infrastructure.Services;

[AutoInterface(Inheritance = [typeof(IBaseService<BookDto, Book>)])]
public class BookService(DataContext dbContext, IMapper mapper)
    : BaseService<BookDto, Book>(dbContext, mapper), IBookService;
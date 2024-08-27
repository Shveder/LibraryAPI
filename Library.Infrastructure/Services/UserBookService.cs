using AutoInterfaceAttributes;
using AutoMapper;
using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.DatabaseContext;
using Library.Infrastructure.Services.Base;

namespace Library.Infrastructure.Services;

[AutoInterface(Inheritance = [typeof(IBaseService<UserBookDto, UserBook>)])]
public class UserBookService(DataContext dbContext, IMapper mapper)
    : BaseService<UserBookDto, UserBook>(dbContext, mapper), IUserBookService;

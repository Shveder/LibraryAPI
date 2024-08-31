using AutoInterfaceAttributes;
using AutoMapper;
using Library.Core.DTO;
using Library.Core.Models;
using Library.Infrastructure.DatabaseContext;
using Library.Infrastructure.Repository;
using Library.Infrastructure.Services.Base;

namespace Library.Infrastructure.Services;

[AutoInterface(Inheritance = [typeof(IBaseService<UserDto, User>)])]
public class UserService(DataContext dbContext, IMapper mapper, IDbRepository repository)
    : BaseService<UserDto, User>(repository, mapper), IUserService;

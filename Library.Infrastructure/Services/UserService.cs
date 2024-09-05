namespace Library.Infrastructure.Services;

[AutoInterface(Inheritance = [typeof(IBaseService<UserDto, User>)])]
public class UserService(DataContext dbContext, IMapper mapper, IDbRepository repository)
    : BaseService<UserDto, User>(repository, mapper), IUserService;
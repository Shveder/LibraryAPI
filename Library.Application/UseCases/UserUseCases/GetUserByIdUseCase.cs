namespace Library.Application.UseCases.UserUseCases;

[AutoInterface(Inheritance = [typeof(IGetByIdUseCase<UserDto, User>)])]
public class GetUserByIdUseCase(IDbRepository repository, IMapper mapper)
    : GetByIdUseCase<UserDto, User>(repository, mapper), IGetUserByIdUseCase;
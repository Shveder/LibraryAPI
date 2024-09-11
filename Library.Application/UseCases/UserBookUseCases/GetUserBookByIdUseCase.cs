namespace Library.Application.UseCases.UserBookUseCases;

[AutoInterface(Inheritance = [typeof(IGetByIdUseCase<UserBookDto, UserBook>)])]
public class GetUserBookByIdUseCase(IDbRepository repository, IMapper mapper)
    : GetByIdUseCase<UserBookDto, UserBook>(repository, mapper), IGetUserBookByIdUseCase;
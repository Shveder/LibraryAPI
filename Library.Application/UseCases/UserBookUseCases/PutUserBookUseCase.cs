namespace Library.Application.UseCases.UserBookUseCases;

[AutoInterface(Inheritance = [typeof(IPutUseCase<UserBookDto, UserBook>)])]
public class PutUserBookUseCase(IDbRepository repository, IMapper mapper)
    : PutUseCase<UserBookDto, UserBook>(repository, mapper), IPutUserBookUseCase;
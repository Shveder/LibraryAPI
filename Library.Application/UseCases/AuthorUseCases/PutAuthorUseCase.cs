namespace Library.Application.UseCases.AuthorUseCases;

[AutoInterface(Inheritance = [typeof(IPutUseCase<AuthorDto, Author>)])]
public class PutAuthorUseCase(IDbRepository repository, IMapper mapper)
    : PutUseCase<AuthorDto, Author>(repository, mapper), IPutAuthorUseCase;
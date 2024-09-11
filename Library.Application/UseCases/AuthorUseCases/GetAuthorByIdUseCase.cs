namespace Library.Application.UseCases.AuthorUseCases;

[AutoInterface(Inheritance = [typeof(IGetByIdUseCase<AuthorDto, Author>)])]
public class GetAuthorByIdUseCase(IDbRepository repository, IMapper mapper)
    : GetByIdUseCase<AuthorDto, Author>(repository, mapper), IGetAuthorByIdUseCase;
namespace Library.Application.UseCases.AuthorUseCases;

[AutoInterface(Inheritance = [typeof(IGetAllUseCase<AuthorDto, Author>)])]
public class GetAllAuthorsUseCase(IDbRepository repository, IMapper mapper)
    : GetAllUseCase<AuthorDto, Author>(repository, mapper), IGetAllAuthorsUseCase;
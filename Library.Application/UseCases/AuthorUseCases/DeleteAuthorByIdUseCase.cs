namespace Library.Application.UseCases.AuthorUseCases;

[AutoInterface(Inheritance = [typeof(IDeleteByIdUseCase<AuthorDto, Author>)])]
public class DeleteAuthorByIdUseCase(IDbRepository repository, IMapper mapper)
    : DeleteByIdUseCase<AuthorDto, Author>(repository, mapper), IDeleteAuthorByIdUseCase;
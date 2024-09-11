namespace Library.Application.UseCases.BookUseCases;

[AutoInterface(Inheritance = [typeof(IDeleteByIdUseCase<BookDto, Book>)])]
public class DeleteBookUseCase(IDbRepository repository, IMapper mapper)
    : DeleteByIdUseCase<BookDto, Book>(repository, mapper), IDeleteBookUseCase;
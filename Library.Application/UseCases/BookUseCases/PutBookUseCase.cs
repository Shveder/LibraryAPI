namespace Library.Application.UseCases.BookUseCases;

[AutoInterface(Inheritance = [typeof(IPutUseCase<BookDto, Book>)])]
public class PutBookUseCase(IDbRepository repository, IMapper mapper)
    : PutUseCase<BookDto, Book>(repository, mapper), IPutBookUseCase;
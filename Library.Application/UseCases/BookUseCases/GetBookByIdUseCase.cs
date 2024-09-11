namespace Library.Application.UseCases.BookUseCases;

[AutoInterface(Inheritance = [typeof(IGetByIdUseCase<BookDto, Book>)])]
public class GetBookByIdUseCase(IDbRepository repository, IMapper mapper)
    : GetByIdUseCase<BookDto, Book>(repository, mapper), IGetBookByIdUseCase
{
    public override async Task<BookDto> GetByIdAsync(Guid id)
    {
        var entity = await repository.Get<Book>(e => e.Id == id)
            .Include(model => model.Author).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = Mapper.Map<BookDto>(entity);
       
        return dto;
    }
}
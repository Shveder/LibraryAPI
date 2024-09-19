namespace Library.Application.UseCases.BookUseCases;

public class GetBookByIdUseCase(IDbRepository repository, IMapper mapper)
{
    public async Task<BookDto> GetByIdAsync(Guid id)
    {
        var entity = await repository.Get<Book>(e => e.Id == id)
            .Include(model => model.Author).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = mapper.Map<BookDto>(entity);
       
        return dto;
    }
}
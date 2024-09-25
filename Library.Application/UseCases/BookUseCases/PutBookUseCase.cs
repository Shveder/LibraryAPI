namespace Library.Application.UseCases.BookUseCases;

public class PutBookUseCase(IDbRepository repository, IMapper mapper)
{
    public async Task<BookDto> PutAsync(BookDto dto)
    {
        var existingBook = repository.Get<Book>(e => e.Id == dto.Id).FirstOrDefault();
        if (existingBook == null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        mapper.Map(dto, existingBook);
        existingBook.DateUpdated = DateTime.UtcNow;
        
        await repository.Update(existingBook);
        await repository.SaveChangesAsync();

        return dto;
    }
}
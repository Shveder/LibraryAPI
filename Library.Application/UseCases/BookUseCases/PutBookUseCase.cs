namespace Library.Application.UseCases.BookUseCases;

public class PutBookUseCase(IDbRepository repository, IMapper mapper)
{
    public virtual async Task<BookDto> PutAsync(BookDto dto)
    {
        var entity = mapper.Map<Book>(dto);
        entity.DateUpdated = DateTime.UtcNow;
        if (!IsBookExists(dto.Id))
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        await repository.Update(entity);
        await repository.SaveChangesAsync();

        return dto;
    }

    private bool IsBookExists(Guid id)
    {
        return repository.Get<Book>(e => e.Id == id).Any();
    }
}
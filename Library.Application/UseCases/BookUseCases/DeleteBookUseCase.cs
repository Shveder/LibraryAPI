namespace Library.Application.UseCases.BookUseCases;

public class DeleteBookUseCase(IDbRepository repository)
{
    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await repository.Get<Book>(e => e.Id == id).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException($"{typeof(Book).Name} {CommonStrings.NotFoundResult}");

        await repository.Delete(entity);
        await repository.SaveChangesAsync();
    }
}
namespace Library.Application.UseCases.AuthorUseCases;

public class DeleteAuthorByIdUseCase(IDbRepository repository)
{
    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await repository.Get<Author>(e => e.Id == id).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException($"{typeof(Author).Name} {CommonStrings.NotFoundResult}");

        await repository.Delete(entity);
        await repository.SaveChangesAsync();
    }
}
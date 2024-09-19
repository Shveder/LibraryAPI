namespace Library.Application.UseCases.UserBookUseCases;

public class DeleteUserBookUseCase(IDbRepository repository)
{
    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = await repository.Get<UserBook>(e => e.Id == id)
            .Include(model=> model.Book)
            .FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException($"{nameof(UserBook)} {CommonStrings.NotFoundResult}");

        var book = await repository.Get<Book>(b => b.Id == entity.Book.Id).FirstOrDefaultAsync();
        if (book is null)
            throw new EntityNotFoundException($"{nameof(UserBook)} {CommonStrings.NotFoundResult}");
        
        book.IsAvailable = true;
        book.DateUpdated = DateTime.UtcNow;
        
        await repository.Update(book);
        await repository.Delete(entity);
        await repository.SaveChangesAsync();
    }
}
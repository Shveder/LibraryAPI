namespace Library.Application.UseCases.UserBookUseCases;

[AutoInterface(Inheritance = [typeof(IDeleteByIdUseCase<UserBookDto, UserBook>)])]
public class DeleteUserBookUseCase(IDbRepository repository, IMapper mapper)
    : DeleteByIdUseCase<UserBookDto, UserBook>(repository, mapper), IDeleteUserBookUseCase
{
    private readonly IDbRepository _repository = repository;

    public override async Task DeleteByIdAsync(Guid id)
    {
        var entity = await _repository.Get<UserBook>(e => e.Id == id)
            .Include(model=> model.Book)
            .FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException($"{nameof(UserBook)} {CommonStrings.NotFoundResult}");

        var book = await _repository.Get<Book>(b => b.Id == entity.Book.Id).FirstOrDefaultAsync();
        if (book is null)
            throw new EntityNotFoundException($"{nameof(UserBook)} {CommonStrings.NotFoundResult}");
        
        book.IsAvailable = true;
        book.DateUpdated = DateTime.UtcNow;
        
        await _repository.Update(book);
        await _repository.Delete(entity);
        await _repository.SaveChangesAsync();
    }
}
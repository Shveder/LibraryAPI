namespace Library.Application.UseCases.UserBookUseCases;

[AutoInterface]
public class GetBookByUserUseCase(IDbRepository repository) : IGetBookByUserUseCase
{
    public async Task<IEnumerable<UserBook>> GetBooksByUserId(Guid userId)
    {
        var user = await repository.Get<User>(u => u.Id == userId).FirstOrDefaultAsync();
        if (user is null)
            throw new IncorrectDataException("User not found");
        
        var userBooks = await repository.GetAll<UserBook>()
            .Where(ub => ub.User== user) 
            .Include(ub => ub.Book)
            .ToListAsync();
        
        if (!userBooks.Any())
            throw new IncorrectDataException("No books found for this user");
        
        return userBooks;
    }
}
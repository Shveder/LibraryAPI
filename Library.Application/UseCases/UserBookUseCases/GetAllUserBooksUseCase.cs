namespace Library.Application.UseCases.UserBookUseCases;

public class GetAllUserBooksUseCase(IDbRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<UserBookDto>> GetAllAsync()
    {
        var entities = await repository.GetAll<UserBook>()
            .Include(model => model.User)
            .Include(model => model.Book).ToListAsync();
        var dtos = mapper.Map<IEnumerable<UserBookDto>>(entities);
        
        return dtos;
    }
}
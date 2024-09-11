namespace Library.Application.UseCases.UserBookUseCases;

[AutoInterface(Inheritance = [typeof(IGetAllUseCase<UserBookDto, UserBook>)])]
public class GetAllUserBooksUseCase(IDbRepository repository, IMapper mapper)
    : GetAllUseCase<UserBookDto, UserBook>(repository, mapper), IGetAllUserBooksUseCase
{
    public override async Task<IEnumerable<UserBookDto>> GetAllAsync()
    {
        var entities = await repository.GetAll<UserBook>()
            .Include(model => model.User)
            .Include(model => model.Book).ToListAsync();
        var dtos = Mapper.Map<IEnumerable<UserBookDto>>(entities);
        
        return dtos;
    }
}
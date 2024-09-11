﻿namespace Library.Application.UseCases.BookUseCases;

[AutoInterface]
public class GetBookByIsbnUseCase(IDbRepository repository, IMapper mapper) : IGetBookByIsbnUseCase
{
    public async Task<BookDto> GetByIsbnAsync(string isbn)
    {
        var entity = await repository.Get<Book>(e => e.ISBN == isbn)
            .Include(model => model.Author).FirstOrDefaultAsync();
        if (entity is null)
            throw new EntityNotFoundException(CommonStrings.NotFoundResult);
        
        var dto = mapper.Map<BookDto>(entity);
       
        return dto;
    }
}
using System.Linq.Expressions;
using Library.Core.Models.Interfaces;

namespace Library.Infrastructure.Repository;

[AutoInterface]
public class DbRepository(DataContext context) : IDbRepository
{
    public IQueryable<T> Get<T>() where T : class, IHasId
    {
        return context.Set<T>().AsQueryable();
    }

    public IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IHasId
    {
        return context.Set<T>().Where(selector).AsQueryable();
    }

    public async Task<Guid> Add<T>(T newEntity) where T : class, IHasId
    {
        var entity = await context.Set<T>().AddAsync(newEntity);
        Console.WriteLine(entity.Entity.Id);
        
        return entity.Entity.Id;
    }
    
    public async Task Delete<T>(T entity) where T : class, IHasId
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task Update<T>(T entity) where T : class, IHasId
    {
        await Task.Run(() => context.Set<T>().Update(entity));
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    public IQueryable<T> GetAll<T>() where T : class, IHasId
    {
        return context.Set<T>().AsQueryable();
    }
}
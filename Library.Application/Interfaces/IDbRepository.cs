namespace Library.Application.Interfaces;

public interface IDbRepository {
    IQueryable<T> Get<T>() where T : class, IHasId;

    IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IHasId;

    Task<Guid> Add<T>(T newEntity) where T : class, IHasId;

    Task Delete<T>(T entity) where T : class, IHasId;

    Task Update<T>(T entity) where T : class, IHasId;

    Task<int> SaveChangesAsync();

    IQueryable<T> GetAll<T>() where T : class, IHasId;
}

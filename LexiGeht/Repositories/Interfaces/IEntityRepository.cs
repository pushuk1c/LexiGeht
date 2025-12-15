
namespace LexiGeht.Repositories.Interfaces
{
    public interface IEntityRepository<T> where T : new()
    {
        Task<T?> GetAsync(object id);
        Task<List<T>> GetAllAsync();
        Task UpsertAsync(T entity);
        Task UpsertManyAsync(IEnumerable<T> items);
        Task Delete(object id);
    }
}

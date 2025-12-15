using LexiGeht.Data;
using LexiGeht.Repositories.Interfaces;
using SQLite;

namespace LexiGeht.Repositories
{
    public class SQLiteRepository<T> : IEntityRepository<T> where T : new()
    {
        private readonly SQLiteAsyncConnection _db;
        private readonly IDatabase _database;
        public SQLiteRepository(IDatabase db)
        {
            _database = db;
            _db = db.Connection;
        }      

        public Task<T?> GetAsync(object id) => _db.FindAsync<T>(id);
        public Task<List<T>> GetAllAsync() => _db.Table<T>().ToListAsync();
        public Task UpsertAsync(T entity) => _db.InsertOrReplaceAsync(entity);
        public async Task UpsertManyAsync(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                await _db.InsertOrReplaceAsync(item);
            }
            
        }
        public Task Delete(object id) => _db.DeleteAsync<T>(id);
    }
}

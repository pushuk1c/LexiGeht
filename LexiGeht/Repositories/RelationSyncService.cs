
using LexiGeht.Data;
using SQLite;

namespace LexiGeht.Repositories
{
    public class RelationSyncService : IRelationSyncService
    {
        private readonly IDatabase _db;
        private readonly SQLiteAsyncConnection _connection;
        public RelationSyncService(IDatabase db)
        {
            _db = db;
            _connection = db.Connection;
        }

        public async Task SyncRelationsAsync(string table, string ownerCol, string itemCol, int ownerId, IEnumerable<int> newItemsIds)
        {
            var oldItemsIds = await _connection.QueryScalarsAsync<int>($"SELECT {itemCol} FROM {table} WHERE {ownerCol} = ?", ownerId);
            var newSet = new HashSet<int>(newItemsIds);
            var toAdd = newSet.Except(oldItemsIds);
            var toRemove = oldItemsIds.Except(newSet);

            foreach (var itemId in toAdd)
                await _connection.ExecuteAsync($"INSERT INTO {table} ({ownerCol}, {itemCol}) VALUES (?, ?)", ownerId, itemId);
                
            foreach (var itemId in toRemove)
                await _connection.ExecuteAsync($"DELETE FROM {table} WHERE {ownerCol} = ? AND {itemCol} = ?", ownerId, itemId);
           
        }
    }
}

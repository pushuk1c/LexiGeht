

namespace LexiGeht.Repositories
{
    public interface IRelationSyncService
    {
        Task SyncRelationsAsync(string table, string ownerCol, string itemCol, int ownerId, IEnumerable<int> newItemsIds);
    }
}

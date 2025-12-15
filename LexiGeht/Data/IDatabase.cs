using SQLite;

namespace LexiGeht.Data
{
    public interface IDatabase
    {
        SQLiteAsyncConnection Connection { get; }
        Task InitializeAsync();
        Task RunInTrx(Func<Task> action);
    }
}

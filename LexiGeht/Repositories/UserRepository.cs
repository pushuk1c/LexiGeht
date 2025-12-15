using LexiGeht.Data;
using LexiGeht.Data.Entities;
using LexiGeht.Models;
using LexiGeht.Repositories.Interfaces;
using SQLite;

namespace LexiGeht.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLiteAsyncConnection _db;

        public UserRepository(IDatabase db)
        {
            _db = db.Connection;
        }

        public Task AddAsync(UserEntity user) => _db.InsertAsync(user);
        public Task UpdateAsync(UserEntity user) => _db.UpdateAsync(user);

        public async Task DeleteAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
                await _db.DeleteAsync(user);            
        }

        public Task<UserEntity> GetByIdAsync(int userId) => _db.FindAsync<UserEntity>(userId);

        public Task<UserEntity> GetByUsernameAsync(string username) => _db.Table<UserEntity>().Where(u => u.Username == username).FirstOrDefaultAsync();


    }
}

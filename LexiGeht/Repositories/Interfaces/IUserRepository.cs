using LexiGeht.Data.Entities;

namespace LexiGeht.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(UserEntity user);
        Task UpdateAsync(UserEntity user);
        Task DeleteAsync(int userId);
        Task<UserEntity> GetByIdAsync(int userId);
        Task<UserEntity> GetByUsernameAsync(string username);

    }
}

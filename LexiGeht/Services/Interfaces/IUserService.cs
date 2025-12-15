using LexiGeht.Common;
using LexiGeht.Models;

namespace LexiGeht.Services.Interfaces
{
    public interface IUserService
    {
        Task<OperationResult<User>> AddUserAsync(string username, string email, string passwordHash);
        Task<OperationResult<User>> UpdateUserAsync(int userId, string username, string email, string passwordHash);
        Task<OperationResult<bool>> DeleteUserAsync(int userId);
        Task<OperationResult<User>> GetUserByNameAsync(string name);
        Task<OperationResult<User>> GetCurrentUserAsync();
        Task<OperationResult<User>> SetCurrentUserAsync(int userId);
        Task<OperationResult<bool>> LogOut();
    }
}

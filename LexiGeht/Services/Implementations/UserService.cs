using LexiGeht.Common;
using LexiGeht.Data.Entities;
using LexiGeht.Data.Mappers;
using LexiGeht.Models;
using LexiGeht.Repositories.Interfaces;
using LexiGeht.Services.Interfaces;

namespace LexiGeht.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private const string CurrentUserKey = "current_user";


        public UserService(IUserRepository userRepository) 
        { 
            _userRepository = userRepository;
        }

        public async Task<OperationResult<User>> AddUserAsync(string username, string email, string passwordHash)
        {
            try
            {
                var existingUser = await _userRepository.GetByUsernameAsync(username);
                if (existingUser != null)
                    return OperationResult<User>.Fail("Username already exists.");

                var userEntity = new UserEntity
                {
                    Username = username,
                    Email = email,
                    PasswordHash = passwordHash
                };

                await _userRepository.AddAsync(userEntity);

                return OperationResult<User>.Ok(UserMapper.ToModel(userEntity));
            }
            catch (Exception ex)
            {
                return OperationResult<User>.Fail($"An error occurred: {ex.Message}");

            }
        }

        public async Task<OperationResult<bool>> DeleteUserAsync(int userId)
        {
            try
            {
                var existingUser = await _userRepository.GetByIdAsync(userId);
                if (existingUser == null)
                    return OperationResult<bool>.Fail("User not found.");

                await _userRepository.DeleteAsync(userId);
                                
                return OperationResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"An error occurred: {ex.Message}");               
            }
        }

        public async Task<OperationResult<User>> UpdateUserAsync(int userId, string username, string email, string passwordHash)
        {
            try
            {
                var existingUser = await _userRepository.GetByIdAsync(userId);
                if (existingUser == null)
                   return OperationResult<User>.Fail("User not found.");

                existingUser.Username = username;
                existingUser.Email = email;
                existingUser.PasswordHash = passwordHash;              

                await _userRepository.UpdateAsync(existingUser);

                return OperationResult<User>.Ok(UserMapper.ToModel(existingUser));
            }
            catch (Exception ex)
            {
                return OperationResult<User>.Fail($"An error occurred: {ex.Message}");
            }           
        }

        public async Task<OperationResult<User>> GetUserByNameAsync(string name)
        {
            try
            {
                var existingUser = await _userRepository.GetByUsernameAsync(name);
                if (existingUser == null)
                    return OperationResult<User>.Fail("User not found.");

                return OperationResult<User>.Ok(UserMapper.ToModel(existingUser));
            }
            catch (Exception ex)
            {
                return OperationResult<User>.Fail($"An error occurred: {ex.Message}");
            }            
        }
        
        public async Task<OperationResult<User>> GetCurrentUserAsync()
        {
            try
            {
                int userID = Preferences.Get(CurrentUserKey, 0);
                if (userID == 0)
                    return OperationResult<User>.Fail("No user is currently logged in.");

                var existingUser = await _userRepository.GetByIdAsync(userID);

                return OperationResult<User>.Ok(UserMapper.ToModel(existingUser));
            }
            catch (Exception ex)
            {
                return OperationResult<User>.Fail($"An error occurred: {ex.Message}");
            }
        }
        
        public async Task<OperationResult<User>> SetCurrentUserAsync(int userId)
        {
            try
            {
                var existingUser = await _userRepository.GetByIdAsync(userId);
                if (existingUser == null)
                    return OperationResult<User>.Fail("User not found.");

                Preferences.Set(CurrentUserKey, userId);
                if (Preferences.Get(CurrentUserKey, 0) != userId)
                    return OperationResult<User>.Fail("Failed to set current user.");

                return OperationResult<User>.Ok(UserMapper.ToModel(existingUser));
            }
            catch (Exception ex)
            {
                return OperationResult<User>.Fail($"An error occurred: {ex.Message}");
            }            
        }

        public async Task<OperationResult<bool>> LogOut()
        {
            try
            {
                int userID = Preferences.Get(CurrentUserKey, 0);
                if (userID == 0)
                    return OperationResult<bool>.Fail("No user is currently logged in.");

                Preferences.Remove(CurrentUserKey);

                return OperationResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }

    }
}

using LexiGeht.Common;
using LexiGeht.Models;
using LexiGeht.Services.Interfaces;


namespace LexiGeht.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;

        public AuthService(IUserService userService, ISecurityService securityService)
        {
            _userService = userService;
            _securityService = securityService;
        }

        public async Task<OperationResult<User>> LoginAsync(string username, string password)
        {
            var userResult = await _userService.GetUserByNameAsync(username);
            if (!userResult.IsSuccess || userResult.Data == null)
                return OperationResult<User>.Fail(userResult.ErrorMessage ?? "Login failed.");
            
            var user = userResult.Data;

            if (!_securityService.VerifyPassword(password, user.PasswordHash))
                return OperationResult<User>.Fail("Invalid password. Login failed");

            var currentUserResult = await _userService.SetCurrentUserAsync(user.Id);
            if (!currentUserResult.IsSuccess)
                return OperationResult<User>.Fail(currentUserResult.ErrorMessage ?? "Login failed");

            return OperationResult<User>.Ok(user);
        }

        public async Task<OperationResult<User>> RegisterAsync(string username, string email, string password)
        {
            return await _userService.AddUserAsync(username, email, _securityService.HashPassword(password));
        }
    }
}

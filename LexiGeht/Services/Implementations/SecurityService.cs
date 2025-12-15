using LexiGeht.Services.Interfaces;

namespace LexiGeht.Services.Implementations
{
    public class SecurityService: ISecurityService
    {
        public string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
        public bool VerifyPassword(string password, string hash)
        {
            return hash == HashPassword(password);
        }

    }
}

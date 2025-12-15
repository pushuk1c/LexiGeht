using LexiGeht.Common;
using LexiGeht.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Services.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult<User>> RegisterAsync(string username, string email, string password);
        Task<OperationResult<User>> LoginAsync(string username, string password);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Services.Interfaces
{
    public interface ISecurityService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hash);
    }
}

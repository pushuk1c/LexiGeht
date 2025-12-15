using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Common
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public T? Data { get; set; }
        
        public static OperationResult<T> Ok(T data) => new() { IsSuccess = true, Data = data };
        public static OperationResult<T> Fail(string error) => new() { IsSuccess = false, ErrorMessage = error };
    }
}

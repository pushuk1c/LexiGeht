using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Services.Interfaces
{
    public interface IGoogleSheetService
    {
        Task<IList<IList<object>>> GetSheetDataAsync(string range);
    }
}

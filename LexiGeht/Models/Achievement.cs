using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Models
{
    class Achievement
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[] IconData { get; set; }
        public string ConditionType { get; set; } = string.Empty;
        public int ConditionValue { get; set; } = 0;
    }
}

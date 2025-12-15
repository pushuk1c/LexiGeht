using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Models
{
    public class Cours
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public List<Quiz> Quizzes { get; set; } = new List<Quiz>();

    }
}

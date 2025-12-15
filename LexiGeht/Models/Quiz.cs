using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public ImageSource ImagePathSource { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public double Progress { get; set; }

    }
}

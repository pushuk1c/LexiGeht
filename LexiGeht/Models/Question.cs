using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public ImageSource ImagePathSource { get; set; }
        public string AudioPath { get; set; } = string.Empty;
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public List<Answer> CorrectAnswers { get; set; } = new List<Answer>();
    }
}

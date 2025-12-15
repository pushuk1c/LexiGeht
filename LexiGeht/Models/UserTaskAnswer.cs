using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Models
{
    class UserTaskAnswer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}

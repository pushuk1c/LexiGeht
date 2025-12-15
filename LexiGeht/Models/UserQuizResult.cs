using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Models
{
    class UserQuizResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}

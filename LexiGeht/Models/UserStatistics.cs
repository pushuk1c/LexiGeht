using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Models
{
    class UserStatistics
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalQuizzesCompleted { get; set; } = 0;
        public int TotalTasksCompleted { get; set; } = 0;
        public int TotalCorrectAnswers { get; set; } = 0;
        public int TotalWrongAnswers { get; set; } = 0;

    }
}

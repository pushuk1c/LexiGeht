using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Models
{
    class UserAchievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AchievementId { get; set; }
        public DateTime DateAchieved { get; set; }
    }
}

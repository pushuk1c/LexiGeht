using SQLite;

namespace LexiGeht.Data.Entities
{
    [Table("Quizzes")]
    public class QuizEntity
    {
        [PrimaryKey] public int Id { get; set; }
        [Indexed] public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

    }
}

using SQLite;

namespace LexiGeht.Data.Entities
{
    [Table("CoursesQuizzes")]
    public class CoursQuizEntity
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        [Indexed] public int CoursId { get; set; }
        [Indexed] public int QuizId { get; set; }

    }
}

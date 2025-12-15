using SQLite;

namespace LexiGeht.Data.Entities
{
    [Table("QuizzesQuestions")]
    public class QuizQuestionEntity
    {
        [PrimaryKey] public int Id { get; set; }
        [Indexed] public int QuizId { get; set; }
        [Indexed] public int QuestionId { get; set; }

    }
}

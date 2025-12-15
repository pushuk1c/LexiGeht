using SQLite;

namespace LexiGeht.Data.Entities
{
    [Table("QuestionsAnswers")]
    public class QuestionAnswerEntity
    {
        [PrimaryKey, AutoIncrement] 
        public int Id { get; set; }
        [Indexed("UX_QuestionsAnswers_QA", 0, Unique = true)] 
        public int QuestionId { get; set; }
        [Indexed("UX_QuestionsAnswers_QA", 0, Unique = true)] 
        public int AnswerId { get; set; }
        public bool IsCorrect{ get; set; }
    }
}

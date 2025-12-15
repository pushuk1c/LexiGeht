using SQLite;

namespace LexiGeht.Data.Entities
{
    [Table("CategoriesQuizzes")]
    public class CategoryQuizEntity
    {
        [PrimaryKey, AutoIncrement ] public int Id { get; set; }
        [Indexed] public int CategoryId { get; set; }
        [Indexed] public int QuizId { get; set; }

    }
}

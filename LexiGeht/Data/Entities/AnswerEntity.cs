using SQLite;

namespace LexiGeht.Data.Entities
{
    [Table("Answers")]
    public class AnswerEntity
    {
        [PrimaryKey] public int Id { get; set; }
        [Indexed] public string Title { get; set; } = string.Empty;
        public string AudioPath { get; set; } = string.Empty;
    }
}

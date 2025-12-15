using SQLite;

namespace LexiGeht.Data.Entities
{
    [Table("Courses")]
    public class CoursEntity
    {
        [PrimaryKey] public int Id { get; set; }
        [Indexed] public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

    }
}

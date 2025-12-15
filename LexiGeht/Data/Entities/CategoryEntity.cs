using SQLite;

namespace LexiGeht.Data.Entities
{
    [Table("Categories")]
    public class CategoryEntity
    {
        [PrimaryKey] public int Id { get; set; }
        [Indexed] public string Title { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}

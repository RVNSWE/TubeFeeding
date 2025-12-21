using SQLite;

namespace TubeFeeding.Models
{
    [Table("Food")]
    public class Food
    {
        [PrimaryKey, AutoIncrement, Column("id"), Unique, NotNull]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Kcal { get; set; }
        public double KcalPerGram { get; set; }
        public double NetWeight { get; set; }
        public double DryWeight { get; set; }
        public double WaterContent { get; set; }
    }
}
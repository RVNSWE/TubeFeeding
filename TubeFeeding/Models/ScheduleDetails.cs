using SQLite;

namespace TubeFeeding.Models
{
    [Table("Schedule")]
    public class Schedule
    {
        [PrimaryKey, AutoIncrement, Column("id"), Unique, NotNull]
        public int Id { get; set; }
        public double BodyWeight { get; set; }
        public double RER { get; set; }
        public double FluidsPerDayTotal { get; set; }
        public double MaxTotalVolumePerMeal { get; set; }
        public double FoodPerDay { get; set; }
        public double FoodPerMeal { get; set; }
        public double WaterPerDay { get; set; }
        public double WaterPerMeal { get; set; }
        public int MealsPerDay { get; set; }
    }
}
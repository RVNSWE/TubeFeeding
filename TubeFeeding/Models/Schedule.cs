using SQLite;

namespace TubeFeeding.Models
{
    [Table("Schedule")]
    public class Schedule
    {
        [PrimaryKey, AutoIncrement, Column("id"), Unique, NotNull]
        public int Id { get; set; }
        public int FoodIdPKey { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty; // Switch to bool - true = cat, false = dog
        // public bool Paediatric { get; set; }
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
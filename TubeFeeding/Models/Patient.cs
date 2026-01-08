using SQLite;

namespace TubeFeeding.Models
{
    [Table("Patient")]
    public class Patient
    {
        [PrimaryKey, AutoIncrement, Column("id"), Unique, NotNull]
        public int Id { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public double KcalPerMl { get; set; } // Doesn't need displaying, but can be used to re-calculate if eg. BW changes
        public double WaterContent { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty; // Switch to bool: true = cat, false = dog
        // public bool Paediatric { get; set; }
        public double BodyWeight { get; set; }
        public double MaxTotalVolumePerMeal { get; set; }
        public double FoodPerMeal { get; set; }
        public double VolPerFlush { get; set; }
        public double WaterToAddPerMeal { get; set; }
        public int MealsPerDay { get; set; }
        public double CansPerDay { get; set; }
    }
}
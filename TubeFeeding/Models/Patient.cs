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
        public double MaxTotalVolumePerMealDayOne { get; set; }
        public double MaxTotalVolumePerMealDayTwo { get; set; }
        public double FoodPerMeal { get; set; }
        public double FoodPerMealDayOne { get; set; }
        public double FoodPerMealDayTwo { get; set; }
        public double VolPerFlush { get; set; }
        public double TotalFluidsPerDay { get; set; }
        public double WaterToAddPerMeal { get; set; }
        public double WaterToAddPerMealDayOne { get; set; }
        public double WaterToAddPerMealDayTwo { get; set; }
        public int MealsPerDay { get; set; }
        public int MealsPerDayOne { get; set; }
        public int MealsPerDayTwo { get; set; }
        public double CansPerDay { get; set; }
        public double CansPerDayOne { get; set; }
        public double CansPerDayTwo { get; set; }
    }
}
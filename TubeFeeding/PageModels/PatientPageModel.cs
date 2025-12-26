using CommunityToolkit.Mvvm.ComponentModel;
using TubeFeeding.Models;

namespace TubeFeeding.PageModels
{
    public partial class PatientPageModel : ObservableObject
    {
        private int _id;
        private string _foodName;
        private double _kcal;
        private double _kcalPerGram;
        private double _netWeight;
        private double _dryWeight;
        private double _waterContent;
        private string _patientName;
        private string _clientName;
        private string _species;
        // private bool _paediatric;
        private double _bodyWeight; // kg
        private double _rER;
        private double _fluidsPerDayTotal;
        private double _maxTotalVolumePerMeal;
        private double _foodPerDay;
        private double _foodPerMeal;
        private double _waterPerDay;
        private double _waterPerMeal;
        private int _mealsPerDay;
        private int _cansPerDay;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string FoodName
        {
            get => _foodName;
            set => SetProperty(ref _foodName, value);
        }

        public double Kcal
        {
            get => _kcal;
            set => SetProperty(ref _kcal, value);
        }

        public double KcalPerGram
        {
            get => _kcalPerGram;
            set => SetProperty(ref _kcalPerGram, value);
        }

        public double NetWeight
        {
            get => _netWeight;
            set => SetProperty(ref _netWeight, value);
        }

        public double DryWeight
        {
            get => _dryWeight;
            set => SetProperty(ref _dryWeight, value);
        }

        public double WaterContent
        {
            get => _waterContent;
            set => SetProperty(ref _waterContent, value);
        }

        public string PatientName
        {
            get => _patientName;
            set => SetProperty(ref _patientName, value);
        }

        public string ClientName
        {
            get => _clientName;
            set => SetProperty(ref _clientName, value);
        }

        public string Species
        {
            get => _species;
            set => SetProperty(ref _species, value);
        }

        /*public bool Paediatric
        {
            get => _paediatric;
            set => SetProperty(ref _paediatric, value);
        }*/

        public double BodyWeight
        {
            get => _bodyWeight;
            set => SetProperty(ref _bodyWeight, value);
        }

        public double RER
        {
            get => _rER;
            set => SetProperty(ref _rER, value);
        }

        public double FluidsPerDayTotal
        {
            get => _fluidsPerDayTotal;
            set => SetProperty(ref _fluidsPerDayTotal, value);
        }

        public double MaxTotalVolumePerMeal
        {
            get => _maxTotalVolumePerMeal;
            set => SetProperty(ref _maxTotalVolumePerMeal, value);
        }

        public double FoodPerDay
        {
            get => _foodPerDay;
            set => SetProperty(ref _foodPerDay, value);
        }

        public double FoodPerMeal
        {
            get => _foodPerMeal;
            set => SetProperty(ref _foodPerMeal, value);
        }

        public double WaterPerDay
        {
            get => _waterPerDay;
            set => SetProperty(ref _waterPerDay, value);
        }

        public double WaterPerMeal
        {
            get => _waterPerMeal;
            set => SetProperty(ref _waterPerMeal, value);
        }

        public int MealsPerDay
        {
            get => _mealsPerDay;
            set => SetProperty(ref _mealsPerDay, value);
        }

        public int CansPerDay
        {
            get => _cansPerDay;
            set => SetProperty(ref _cansPerDay, value);
        }

        public PatientPageModel(Patient model)
        {
            _id = model.Id;
            _foodName = model.FoodName;
            _kcal = model.Kcal;
            _kcalPerGram = model.KcalPerGram;
            _netWeight = model.NetWeight;
            _dryWeight = model.DryWeight;
            _waterContent = model.WaterContent;
            _patientName = model.PatientName;
            _clientName = model.ClientName;
            _species = model.Species;
            // _paediatric = model.Paediatric;
            _bodyWeight = model.BodyWeight;
            _rER = model.RER;
            _fluidsPerDayTotal = model.FluidsPerDayTotal;
            _maxTotalVolumePerMeal = model.MaxTotalVolumePerMeal;
            _foodPerDay = model.FoodPerDay;
            _foodPerMeal = model.FoodPerMeal;
            _waterPerDay = model.WaterPerDay;
            _waterPerMeal = model.WaterPerMeal;
            _mealsPerDay = model.MealsPerDay;
            _cansPerDay = model.CansPerDay;
        }

        /*public async Task ComposeSchedule()
        {
            double totalVolumePerMeal = FoodPerMeal + WaterPerMeal;
            double totalVolumePerDay = FoodPerDay + WaterPerDay;
            int minMealsPerDay = (int)totalVolumePerDay / (int)totalVolumePerMeal;

            int hour;
            int timeOffset;

            if (MealsPerDay < minMealsPerDay)
            {
                timeOffset = minMealsPerDay / 2;
            }
            else
            {
                timeOffset = MealsPerDay / 2;
            }
            hour = 12 - timeOffset;

            if (totalVolumePerMeal > MaxTotalVolumePerMeal)
            {
                CalculateFeedingTimes(minMealsPerDay, hour);
            }
            else
            {
                CalculateFeedingTimes(MealsPerDay, hour);
            }
        }

        public void CalculateFeedingTimes(int mealsPerDay, int startingHour)
        {
            int timeIncrement = 12 / mealsPerDay;
            int hour = startingHour;

            for (int i = 0; i < mealsPerDay; i++)
            {
                FeedingTimes[i] = hour;
                hour += timeIncrement;
            }
        }*/
    }
}
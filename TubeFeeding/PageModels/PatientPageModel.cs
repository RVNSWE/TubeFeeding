using CommunityToolkit.Mvvm.ComponentModel;
using TubeFeeding.Models;

namespace TubeFeeding.PageModels
{
    public partial class PatientPageModel : ObservableObject
    {
        private int _id;
        private string _foodName;
        private double _kcalPerMl;
        private double _waterContent;
        private string _patientName;
        private string _clientName;
        private string _species;
        // private bool _paediatric;
        private double _bodyWeight; // kg
        private double _maxTotalVolumePerMeal;
        private double _foodPerMeal;
        private double _flushPerMeal;
        private double _waterToAddPerMeal;
        private int _mealsPerDay;
        private double _cansPerDay;

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

        public double KcalPerMl
        {
            get => _kcalPerMl;
            set => SetProperty(ref _kcalPerMl, value);
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

        public double MaxTotalVolumePerMeal
        {
            get => _maxTotalVolumePerMeal;
            set => SetProperty(ref _maxTotalVolumePerMeal, value);
        }

        public double FoodPerMeal
        {
            get => _foodPerMeal;
            set => SetProperty(ref _foodPerMeal, value);
        }

        public double FlushPerMeal
        {
            get => _flushPerMeal;
            set => SetProperty(ref _flushPerMeal, value);
        }

        public double WaterToAddPerMeal
        {
            get => _waterToAddPerMeal;
            set => SetProperty(ref _waterToAddPerMeal, value);
        }

        public int MealsPerDay
        {
            get => _mealsPerDay;
            set => SetProperty(ref _mealsPerDay, value);
        }

        public double CansPerDay
        {
            get => _cansPerDay;
            set => SetProperty(ref _cansPerDay, value);
        }

        public PatientPageModel(Patient model)
        {
            _id = model.Id;
            _foodName = model.FoodName;
            _kcalPerMl = model.KcalPerMl;
            _waterContent = model.WaterContent;
            _patientName = model.PatientName;
            _clientName = model.ClientName;
            _species = model.Species;
            // _paediatric = model.Paediatric;
            _bodyWeight = model.BodyWeight;
            _maxTotalVolumePerMeal = model.MaxTotalVolumePerMeal;
            _foodPerMeal = model.FoodPerMeal;
            _flushPerMeal = model.FlushPerMeal;
            _waterToAddPerMeal = model.WaterToAddPerMeal;
            _mealsPerDay = model.MealsPerDay;
            _cansPerDay = model.CansPerDay;
        }
    }
}
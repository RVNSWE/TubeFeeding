using CommunityToolkit.Mvvm.ComponentModel;
using TubeFeeding.Models;

namespace TubeFeeding.PageModels
{
    public partial class ScheduleDetailsPageModel : ObservableObject
    {
        private int _id;
        private double _bodyWeight; // kg
        private double _rER;
        private double _fluidsPerDayTotal;
        private double _maxTotalVolumePerMeal;
        private double _foodPerDay;
        private double _foodPerMeal;
        private double _waterPerDay;
        private double _waterPerMeal;
        private int _mealsPerDay;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

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

        public ScheduleDetailsPageModel(Schedule model)
        {
            _id = model.Id;
            _bodyWeight = model.BodyWeight;
            _rER = model.RER;
            _fluidsPerDayTotal = model.FluidsPerDayTotal;
            _maxTotalVolumePerMeal = model.MaxTotalVolumePerMeal;
            _foodPerDay = model.FoodPerDay;
            _foodPerMeal = model.FoodPerMeal;
            _waterPerDay = model.WaterPerDay;
            _waterPerMeal = model.WaterPerMeal;
            _mealsPerDay = model.MealsPerDay;
        }
    }
}
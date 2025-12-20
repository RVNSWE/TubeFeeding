using CommunityToolkit.Mvvm.ComponentModel;
using TubeFeeding.Models;

namespace TubeFeeding.PageModels
{
    public partial class FoodPageModel : ObservableObject
    {
        private int _id;
        private string _name;
        private double _kcal;
        private double _grams;
        private double _kcalPerGram;
        private double _netWeight;
        private double _dryWeight;
        private double _waterContent;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public double Kcal
        {
            get => _kcal;
            set => SetProperty(ref _kcal, value);
        }

        public double Grams
        {
            get => _grams;
            set => SetProperty(ref _grams, value);
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

        public FoodPageModel(Food model)
        {
            _id = model.Id;
            _kcal = model.Kcal;
            _grams = model.Grams;
            _kcalPerGram = model.KcalPerGram;
            _netWeight = model.NetWeight;
            _dryWeight = model.DryWeight;
            _waterContent = model.WaterContent;
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TubeFeeding.Models;
using TubeFeeding.Pages.Controls;

namespace TubeFeeding.PageModels
{
    public partial class PatientPageModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
        private double _maxTotalVolumePerMealDayOne;
        private double _maxTotalVolumePerMealDayTwo;
        private double _foodPerMeal;
        private double _foodPerMealDayOne;
        private double _foodPerMealDayTwo;
        private double _volPerFlush;
        private double _waterToAddPerMeal;
        private double _waterToAddPerMealDayOne;
        private double _waterToAddPerMealDayTwo;
        private int _mealsPerDay;
        private int _mealsPerDayOne;
        private int _mealsPerDayTwo;
        private double _cansPerDay;
        private double _cansPerDayOne;
        private double _cansPerDayTwo;

        private string _generatingPdf;

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

        public double MaxTotalVolumePerMealDayOne
        {
            get => _maxTotalVolumePerMealDayOne;
            set => SetProperty(ref _maxTotalVolumePerMealDayOne, value);
        }

        public double MaxTotalVolumePerMealDayTwo
        {
            get => _maxTotalVolumePerMealDayTwo;
            set => SetProperty(ref _maxTotalVolumePerMealDayTwo, value);
        }

        public double FoodPerMeal
        {
            get => _foodPerMeal;
            set => SetProperty(ref _foodPerMeal, value);
        }

        public double FoodPerMealDayOne
        {
            get => _foodPerMealDayOne;
            set => SetProperty(ref _foodPerMealDayOne, value);
        }

        public double FoodPerMealDayTwo
        {
            get => _foodPerMealDayTwo;
            set => SetProperty(ref _foodPerMealDayTwo, value);
        }

        public double VolPerFlush
        {
            get => _volPerFlush;
            set => SetProperty(ref _volPerFlush, value);
        }

        public double WaterToAddPerMeal
        {
            get => _waterToAddPerMeal;
            set => SetProperty(ref _waterToAddPerMeal, value);
        }

        public double WaterToAddPerMealDayOne
        {
            get => _waterToAddPerMealDayOne;
            set => SetProperty(ref _waterToAddPerMealDayOne, value);
        }

        public double WaterToAddPerMealDayTwo
        {
            get => _waterToAddPerMealDayTwo;
            set => SetProperty(ref _waterToAddPerMealDayTwo, value);
        }

        public int MealsPerDay
        {
            get => _mealsPerDay;
            set => SetProperty(ref _mealsPerDay, value);
        }

        public int MealsPerDayOne
        {
            get => _mealsPerDayOne;
            set => SetProperty(ref _mealsPerDayOne, value);
        }

        public int MealsPerDayTwo
        {
            get => _mealsPerDayTwo;
            set => SetProperty(ref _mealsPerDayTwo, value);
        }

        public double CansPerDay
        {
            get => _cansPerDay;
            set => SetProperty(ref _cansPerDay, value);
        }

        public double CansPerDayOne
        {
            get => _cansPerDayOne;
            set => SetProperty(ref _cansPerDayOne, value);
        }

        public double CansPerDayTwo
        {
            get => _cansPerDayTwo;
            set => SetProperty(ref _cansPerDayTwo, value);
        }

        public string GeneratingPdf
        {
            get => _generatingPdf;
            set
            {
                if (_generatingPdf != value)
                {
                    _generatingPdf = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FilePath { get; set; }
        public string NameString { get; private set; }
        public string WeightString { get; private set; }
        public string KcalString { get; private set; }
        public string MaxVolString { get; private set; }
        public string MaxVolDayOneString { get; private set; }
        public string MaxVolDayTwoString { get; private set; }
        public string FoodPerMealString { get; private set; }
        public string FoodPerMealDayOneString { get; private set; }
        public string FoodPerMealDayTwoString { get; private set; }
        public string WaterPerMealString { get; private set; }
        public string WaterPerMealDayOneString { get; private set; }
        public string WaterPerMealDayTwoString { get; private set; }
        public string FlushString { get; private set; }

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
            _maxTotalVolumePerMealDayOne = model.MaxTotalVolumePerMealDayOne;
            _maxTotalVolumePerMealDayTwo = model.MaxTotalVolumePerMealDayTwo;
            _foodPerMeal = model.FoodPerMeal;
            _foodPerMealDayOne = model.FoodPerMealDayOne;
            _foodPerMealDayTwo = model.FoodPerMealDayTwo;
            _volPerFlush = model.VolPerFlush;
            _waterToAddPerMeal = model.WaterToAddPerMeal;
            _waterToAddPerMealDayOne = model.WaterToAddPerMealDayOne;
            _waterToAddPerMealDayTwo = model.WaterToAddPerMealDayTwo;
            _mealsPerDay = model.MealsPerDay;
            _mealsPerDayOne = model.MealsPerDayOne;
            _mealsPerDayTwo = model.MealsPerDayTwo;
            _cansPerDay = model.CansPerDay;
            _cansPerDayOne = model.CansPerDayOne;
            _cansPerDayTwo = model.CansPerDayTwo;

            NameString = $"{_patientName} {_clientName}";
            WeightString = $"{_bodyWeight} kg";
            KcalString = $"{_kcalPerMl} kcal/ml";
            MaxVolDayOneString = $"{_maxTotalVolumePerMealDayOne} ml";
            MaxVolDayTwoString = $"{_maxTotalVolumePerMealDayTwo} ml";
            MaxVolString = $"{_maxTotalVolumePerMeal} ml";
            FoodPerMealDayOneString = $"{_foodPerMealDayOne} ml";
            FoodPerMealDayTwoString = $"{_foodPerMealDayTwo} ml";
            FoodPerMealString = $"{_foodPerMeal} ml";
            WaterPerMealDayOneString = $"{_waterToAddPerMealDayOne} ml";
            WaterPerMealDayTwoString = $"{_waterToAddPerMealDayTwo} ml";
            WaterPerMealString = $"{_waterToAddPerMeal} ml";
            FlushString = $"{_volPerFlush} ml";

            GeneratingPdf = "";
            _generatingPdf = GeneratingPdf;
            FilePath = "";
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public async Task GeneratePdf()
        {
            try
            {
#if WINDOWS
                string pdfPath = FilePath + $"\\{PatientName}_{ClientName}_{FoodName}.pdf";
#else
                string pdfPath = Globals.GetLocalPath($"{PatientName}_{ClientName}_{FoodName}.pdf");
#endif
                FeedingSchedule feedingSchedule = new(this);
                ExportDoc output = new(feedingSchedule, pdfPath);

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = $"{PatientName} {ClientName} - Tube Feeding Plan",
                    File = new ShareFile(pdfPath)
                });

                GeneratingPdf = "PDF creation successful. File location: " + pdfPath;
                System.Diagnostics.Debug.WriteLine("PDF creation successful.");
            }
            catch (Exception ex)
            {
                GeneratingPdf = "PDF generation failed. Error: " + ex.Message;
                System.Diagnostics.Debug.WriteLine("PDF generation failed. Error: " + ex.Message);
            }
        }
    }
}
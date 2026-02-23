using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TubeFeeding.Models;
using TubeFeeding.Pages.Controls;

namespace TubeFeeding.PageModels
{
    public partial class AddPatientPageModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Patient patient;
        private const int MAX_ML_PER_KG_DAY_ONE = 10;
        private const int MAX_ML_PER_KG_DAY_TWO = 15;
        private const int MAX_ML_PER_KG = 20;
        private double totalVolumePerMealDayOne;
        private double totalVolumePerMealDayTwo;
        private double totalVolumePerMeal;
        private double minWaterPerDayOne;
        private double minWaterPerDayTwo;
        private double minWaterPerDay;
        private double totalFoodAndWaterPerDayOne;
        private double totalFoodAndWaterPerDayTwo;
        private double totalFoodAndWaterPerDay;
        private double foodNetWeight;

        private string _patientNameHelper;
        private string _clientNameHelper;
        private string _speciesHelper;
        private string _bodyWeightHelper;
        private string _dietHelper;
        private string _kcalHelper;
        private string _netWeightHelper;
        private string _percentWaterHelper;
        private string _validationFailureMessage;

        private readonly string enterNumber;
        private readonly string patientNameErrorMessage;
        private readonly string clientNameErrorMessage;
        private readonly string speciesErrorMessage;
        private readonly string dietErrorMessage;

        public string PatientNameHelper
        {
            get => _patientNameHelper;
            set
            {
                if (_patientNameHelper != value)
                {
                    _patientNameHelper = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ClientNameHelper
        {
            get => _clientNameHelper;
            set
            {
                if (_clientNameHelper != value)
                {
                    _clientNameHelper = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SpeciesHelper
        {
            get => _speciesHelper;
            set
            {
                if (_speciesHelper != value)
                {
                    _speciesHelper = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BodyWeightHelper
        {
            get => _bodyWeightHelper;
            set
            {
                if (_bodyWeightHelper != value)
                {
                    _bodyWeightHelper = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DietHelper
        {
            get => _dietHelper;
            set
            {
                if (_dietHelper != value)
                {
                    _dietHelper = value;
                    OnPropertyChanged();
                }
            }
        }

        public string KcalHelper
        {
            get => _kcalHelper;
            set
            {
                if (_kcalHelper != value)
                {
                    _kcalHelper = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NetWeightHelper
        {
            get => _netWeightHelper;
            set
            {
                if (_netWeightHelper != value)
                {
                    _netWeightHelper = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PercentWaterHelper
        {
            get => _percentWaterHelper;
            set
            {
                if (_percentWaterHelper != value)
                {
                    _percentWaterHelper = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ValidationFailureMessage
        {
            get => _validationFailureMessage;
            set
            {
                if (_validationFailureMessage != value)
                {
                    _validationFailureMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public AddPatientPageModel()
        {
            InitialiseNewPatient();
            enterNumber = "Please enter either a whole number or decimal";
            patientNameErrorMessage = "Please enter the patient's name";
            clientNameErrorMessage = "Please enter the client's name";
            speciesErrorMessage = "Please select the patient's species";
            dietErrorMessage = "Please enter the name of the food";
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void InitialiseNewPatient()
        {
            patient = new();
            PatientNameHelper = "";
            ClientNameHelper = "";
            SpeciesHelper = "";
            BodyWeightHelper = "";
            DietHelper = "";
            KcalHelper = "";
            NetWeightHelper = "";
            PercentWaterHelper = "";
            ValidationFailureMessage = "";
        }

        private bool InputValid(string rawBodyWeight, string rawKcal, string rawNetWeight, string rawWaterPercentage)
        {
            bool isValid = true;

            if (Globals.IsStringEmpty(patient.PatientName))
            {
                PatientNameHelper = patientNameErrorMessage;
                isValid = false;
            }

            if (Globals.IsStringEmpty(patient.ClientName))
            {
                ClientNameHelper = clientNameErrorMessage;
                isValid = false;
            }

            if (Globals.IsStringEmpty(patient.Species) || patient.Species == "None")
            {
                SpeciesHelper = speciesErrorMessage;
                isValid = false;
            }

            if (Globals.IsStringEmpty(patient.FoodName))
            {
                DietHelper = dietErrorMessage;
                isValid = false;
            }

            if (Globals.IsStringEmpty(rawBodyWeight) || !double.TryParse(rawBodyWeight, out double bodyWeight))
            {
                BodyWeightHelper = enterNumber;
                isValid = false;
            }
            else
            {
                patient.BodyWeight = bodyWeight;
            }

            if (Globals.IsStringEmpty(rawKcal) || !double.TryParse(rawKcal, out double kcal))
            {
                KcalHelper = enterNumber;
                isValid = false;
            }
            else
            {
                patient.KcalPerMl = kcal * 0.001;
            }

            if (Globals.IsStringEmpty(rawNetWeight) || !double.TryParse(rawNetWeight, out double netWeight))
            {
                NetWeightHelper = enterNumber;
                isValid = false;
            }
            else
            {
                foodNetWeight = netWeight;
            }

            if (Globals.IsStringEmpty(rawWaterPercentage) || !double.TryParse(rawWaterPercentage, out double waterPercentage))
            {
                PercentWaterHelper = enterNumber;
                isValid = false;
            }
            else
            {
                patient.WaterContent = waterPercentage * 0.01;
            }

            return isValid;
        }


        public async Task SaveNewSchedule(
            string newPatientNameText,
            string newClientNameText,
            string speciesLabelText,
            string newBodyWeightText,
            string newFoodNameText,
            string newKcalText,
            string newNetWeightText,
            string newWaterPercentageText
            )
        {
            InitialiseNewPatient();
            patient.PatientName = Globals.FormatString(newPatientNameText);
            patient.ClientName = Globals.FormatString(newClientNameText);
            patient.FoodName = Globals.FormatString(newFoodNameText);
            patient.Species = Globals.FormatString(speciesLabelText);
            string rawBodyWeight = Globals.FormatString(newBodyWeightText);
            string rawKcal = Globals.FormatString(newKcalText);
            string rawNetWeight = Globals.FormatString(newNetWeightText);
            string rawWaterPercentage = Globals.FormatString(newWaterPercentageText);

            if (!InputValid(rawBodyWeight, rawKcal, rawNetWeight, rawWaterPercentage))
            {
                ValidationFailureMessage = "Please address the errors highlighted above and then try again";
            }
            else
            {
                Globals.GoToList(); // So "back" from the details page navigates back to the list instead of AddPatientPage

                double rER = Globals.CalculateRER(patient.BodyWeight);
                double foodPerDay = rER / patient.KcalPerMl;
                double foodPerDayOne = foodPerDay * 0.33;
                double foodPerDayTwo = foodPerDay * 0.66;
                double foodWaterContentDayOne = foodPerDayOne * patient.WaterContent;
                double foodWaterContentDayTwo = foodPerDayTwo * patient.WaterContent;
                double foodWaterContent = foodPerDay * patient.WaterContent;
                patient.MaxTotalVolumePerMealDayOne = patient.BodyWeight * MAX_ML_PER_KG_DAY_ONE;
                patient.MaxTotalVolumePerMealDayTwo = patient.BodyWeight * MAX_ML_PER_KG_DAY_TWO;
                patient.MaxTotalVolumePerMeal = patient.BodyWeight * MAX_ML_PER_KG;
                double flushPerMeal = Globals.GetFlushPerMeal(patient.BodyWeight);
                double totalFluidsCalcOne;
                double totalFluidsCalcTwo;

                if (patient.Species == "Cat")
                {
                    totalFluidsCalcOne = Globals.FluidCalculationOne(40, patient.BodyWeight);
                    totalFluidsCalcTwo = Globals.FluidCalculationTwo(80, patient.BodyWeight);
                }
                else
                {
                    totalFluidsCalcOne = Globals.FluidCalculationOne(60, patient.BodyWeight);
                    totalFluidsCalcTwo = Globals.FluidCalculationTwo(132, patient.BodyWeight);
                }

                minWaterPerDayOne = Globals.GetMinWaterPerDay(totalFluidsCalcOne, totalFluidsCalcTwo, foodWaterContentDayOne);
                minWaterPerDayTwo = Globals.GetMinWaterPerDay(totalFluidsCalcOne, totalFluidsCalcTwo, foodWaterContentDayTwo);
                minWaterPerDay = Globals.GetMinWaterPerDay(totalFluidsCalcOne, totalFluidsCalcTwo, foodWaterContent);

                totalFoodAndWaterPerDayOne = foodPerDayOne + minWaterPerDayOne;
                totalFoodAndWaterPerDayTwo = foodPerDayTwo + minWaterPerDayTwo;
                totalFoodAndWaterPerDay = foodPerDay + minWaterPerDay;

                patient.MealsPerDayOne = (int)Math.Round(totalFoodAndWaterPerDayOne / patient.MaxTotalVolumePerMealDayOne, 0, MidpointRounding.AwayFromZero);
                patient.MealsPerDayTwo = (int)Math.Round(totalFoodAndWaterPerDayTwo / patient.MaxTotalVolumePerMealDayTwo, 0, MidpointRounding.AwayFromZero);
                patient.MealsPerDay = (int)Math.Round(totalFoodAndWaterPerDay / patient.MaxTotalVolumePerMeal, 0, MidpointRounding.AwayFromZero);

                CalculateMealsForDay(1, foodPerDayOne, flushPerMeal, patient.MealsPerDayOne, minWaterPerDayOne);
                CalculateMealsForDay(2, foodPerDayTwo, flushPerMeal, patient.MealsPerDayTwo, minWaterPerDayTwo);
                CalculateMealsForDay(3, foodPerDay, flushPerMeal, patient.MealsPerDay, minWaterPerDay);

                while (totalVolumePerMealDayOne > patient.MaxTotalVolumePerMealDayOne)
                {
                    patient.MealsPerDayOne += 1;

                    CalculateMealsForDay(1, foodPerDayOne, flushPerMeal, patient.MealsPerDayOne, minWaterPerDayOne);

                    if (patient.MealsPerDayOne > 23)
                    {
                        break;
                    }
                }
                while (totalVolumePerMealDayTwo > patient.MaxTotalVolumePerMealDayTwo)
                {
                    patient.MealsPerDayTwo += 1;

                    CalculateMealsForDay(2, foodPerDayTwo, flushPerMeal, patient.MealsPerDayTwo, minWaterPerDayTwo);

                    if (patient.MealsPerDayTwo > 23)
                    {
                        break;
                    }
                }
                while (totalVolumePerMeal > patient.MaxTotalVolumePerMeal)
                {
                    patient.MealsPerDay += 1;

                    CalculateMealsForDay(3, foodPerDay, flushPerMeal, patient.MealsPerDay, minWaterPerDay);

                    if (patient.MealsPerDay > 23)
                    {
                        break;
                    }
                }

                patient.CansPerDayOne = foodPerDayOne / foodNetWeight;
                patient.CansPerDayTwo = foodPerDayTwo / foodNetWeight;
                patient.CansPerDay = foodPerDay / foodNetWeight;
                patient.VolPerFlush = flushPerMeal / 2;

                patient.KcalPerMl = Math.Round(patient.KcalPerMl, 3, MidpointRounding.AwayFromZero);
                patient.MaxTotalVolumePerMeal = Math.Round(patient.MaxTotalVolumePerMeal, 1, MidpointRounding.AwayFromZero);
                patient.MaxTotalVolumePerMealDayOne = Math.Round(patient.MaxTotalVolumePerMealDayOne, 1, MidpointRounding.AwayFromZero);
                patient.MaxTotalVolumePerMealDayTwo = Math.Round(patient.MaxTotalVolumePerMealDayTwo, 1, MidpointRounding.AwayFromZero);
                patient.FoodPerMeal = Math.Round(patient.FoodPerMeal, 1, MidpointRounding.AwayFromZero);
                patient.FoodPerMealDayOne = Math.Round(patient.FoodPerMealDayOne, 1, MidpointRounding.AwayFromZero);
                patient.FoodPerMealDayTwo = Math.Round(patient.FoodPerMealDayTwo, 1, MidpointRounding.AwayFromZero);
                patient.WaterToAddPerMeal = Math.Round(patient.WaterToAddPerMeal, 1, MidpointRounding.AwayFromZero);
                patient.WaterToAddPerMealDayOne = Math.Round(patient.WaterToAddPerMealDayOne, 1, MidpointRounding.AwayFromZero);
                patient.WaterToAddPerMealDayTwo = Math.Round(patient.WaterToAddPerMealDayTwo, 1, MidpointRounding.AwayFromZero);
                patient.CansPerDay = Math.Round(patient.CansPerDay, 1, MidpointRounding.AwayFromZero);
                patient.CansPerDayOne = Math.Round(patient.CansPerDayOne, 1, MidpointRounding.AwayFromZero);
                patient.CansPerDayTwo = Math.Round(patient.CansPerDayTwo, 1, MidpointRounding.AwayFromZero);

                await App.Repo.AddNewSchedule(patient);

                Globals.GoToView();
            }
        }

        private void CalculateMealsForDay(int day, double foodPerDay, double flushPerMeal, double mealsPerDay, double minWater)
        {
            double minWaterMinusFlush = minWater - (flushPerMeal * mealsPerDay);

            if (day == 1)
            {
                minWaterPerDayOne = minWaterMinusFlush;

                if (minWaterPerDayOne < 0)
                {
                    minWaterPerDayOne = 0;
                }

                patient.FoodPerMealDayOne = foodPerDay / mealsPerDay;
                patient.WaterToAddPerMealDayOne = minWaterPerDayOne / mealsPerDay;

                totalFoodAndWaterPerDayOne = foodPerDay + minWaterPerDayOne;
                totalVolumePerMealDayOne = patient.FoodPerMealDayOne + flushPerMeal + patient.WaterToAddPerMealDayOne;
            }
            if (day == 2)
            {
                minWaterPerDayTwo = minWaterMinusFlush;

                if (minWaterPerDayTwo < 0)
                {
                    minWaterPerDayTwo = 0;
                }

                patient.FoodPerMealDayTwo = foodPerDay / mealsPerDay;
                patient.WaterToAddPerMealDayTwo = minWaterPerDayTwo / mealsPerDay;

                totalFoodAndWaterPerDayTwo = foodPerDay + minWaterPerDayTwo;
                totalVolumePerMealDayTwo = patient.FoodPerMealDayTwo + flushPerMeal + patient.WaterToAddPerMealDayTwo;
            }
            if (day == 3)
            {
                minWaterPerDay = minWaterMinusFlush;

                if (minWaterPerDay < 0)
                {
                    minWaterPerDay = 0;
                }

                patient.FoodPerMeal = foodPerDay / mealsPerDay;
                patient.WaterToAddPerMeal = minWaterPerDay / mealsPerDay;

                totalFoodAndWaterPerDay = foodPerDay + minWaterPerDay;
                totalVolumePerMeal = patient.FoodPerMeal + flushPerMeal + patient.WaterToAddPerMeal;
            }
        }
    }
}